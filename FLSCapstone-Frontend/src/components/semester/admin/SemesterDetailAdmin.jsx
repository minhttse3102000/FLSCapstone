import { ArrowBackIosNew, Check, HorizontalRule } from '@mui/icons-material'
import { Button, IconButton, Stack, Tooltip, Typography } from '@mui/material'
import { blue, green, grey } from '@mui/material/colors'
import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import request from '../../../utils/request'
import Title from '../../title/Title'
import LecturerContainer from '../manager/LecturerContainer'
import ConfirmModal from './ConfirmModal'
import CourseList from './CourseList'
import SlotType from './SlotType'
import { ToastContainer, toast } from 'react-toastify';
import SummarySubject from './SummarySubject'
import ViewConfirm from './ViewConfirm'

const SemesterDetailAdmin = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [selected, setSelected] = useState('courses')
  const [semester, setSemester] = useState({});
  const [isConfirm, setIsConfirm] = useState(false);
  const [content, setContent] = useState('');
  const [mode, setMode] = useState('');
  const [schedule, setSchedule] = useState({});
  const [slotTypes, setSlotTypes] = useState([]);
  const [checkAllConfirm, setCheckAllConfirm] = useState({});
  const [reCheckAll, setReCheckAll] = useState(false);

  //set semester
  useEffect(() => {
    request.get(`Semester/${id}`)
      .then(res => {
        if (res.data) {
          setSemester(res.data);
        }
      })
      .catch(err => {
        alert('Fail to load semester')
      })
  }, [id, isConfirm])

  //set schedule
  useEffect(() => {
    request.get('Schedule', {
      params: {SemesterId: id, pageIndex: 1, pageSize: 10}
    })
    .then(res => {
      if(res.data.length > 0) setSchedule(res.data[0])
    })
    .catch(err => alert('Fail to get schedule'))
  }, [id])

  //get list slot types by semester
  useEffect(() => {
    request.get('SlotType', {
      params: {
        SemesterId: id, sortBy: 'DayOfWeekAndTimeStart', order: 'Asc',
        pageIndex: 1, pageSize: 100
      }
    }).then(res => {
      if (res.data) setSlotTypes(res.data);
    }).catch(err => alert('Fail to load slottype'))
  }, [id])

  //check all manager confirmed
  useEffect(() => {
    if(id){
      request.get(`CheckConstraint/CheckAllDepartmentManagerConfirm/${id}`)
      .then(res => {
        if(res.data){
          setCheckAllConfirm(res.data)
        }
      })
      .catch(err => {alert('Fail to check managers confirmed')})
    }
  }, [id, reCheckAll])

  const backToSemester = () => {
    navigate('/admin/semester')
  }

  const clickNextState = () => {
    if(semester.State === 6) return;

    setMode('next');
    if(semester.State === 1) setContent('Next state is Voting. Lecturers can rate subjects, slots and send requests.')
    else if(semester.State === 2) setContent('Next state is Evaluating. Department Managers can evaluate subjects, courses and slots to Lecturers. ')
    else if(semester.State === 3) setContent('Next state is Blocked. All functions are blocked to begin generating schedule.')
    else if(semester.State === 4) setContent('Next state is Adjusting. The schedule will be shown for Lecturers and Department Managers. The Managers can adjust the schedule.')
    else setContent('Next state is Public. The schedule will be public and can not be edited.')
    
    setIsConfirm(true);
  }

  const clickPrevState = () => {
    if(semester.State === 1) return;

    setMode('prev')
    if(semester.State === 6) setContent('Previous state is Adjusting. The schedule will be shown for Lecturers and Department Managers. The Managers can adjust the schedule.')
    else if(semester.State === 5) setContent('Previous state is Blocked. All functions are blocked to begin generating schedule.')
    else if(semester.State === 4) setContent('Previous state is Evaluating. Department Managers can evaluate subjects, courses and slots to Lecturers.')
    else if(semester.State === 3) setContent('Previous state is Voting. Lecturers can rate subjects, slots and send requests.')
    else setContent('Previous state is New.')
    setIsConfirm(true);
  }

  const saveNextState = () => {
    if(semester.State === 6) return;

    if(semester.State === 5){
      request.put(`Schedule/${schedule.Id}`, {
        IsPublic: 1, SemesterId: id,
        Description: '', DateCreate: ''
      }).then(res => {

      }).catch(err => {alert('Fail to public schedule')})
    }

    request.put(`Semester/${id}`, {
      Term: semester.Term, DateStart: semester.DateStart,
      DateEnd: semester.DateEnd, State: (semester.State + 1)
    }).then(res => {
      if (res.status === 200) {
        setIsConfirm(false)
        toast.success('Success to change next state!', {
          position: "top-right", autoClose: 2000, hideProgressBar: false, closeOnClick: true,
          pauseOnHover: true, draggable: true, progress: undefined, theme: "light",
        });
      }
    }).catch(err => {
      alert('Fail to change next state')
      setIsConfirm(false)
    })
  }

  const savePrevState = () => {
    if(semester.State === 1) return;

    if(semester.State === 6){
      request.put(`Schedule/${schedule.Id}`, {
        IsPublic: 0, SemesterId: id,
        Description: '', DateCreate: ''
      }).then(res => {

      }).catch(err => {alert('Fail to update schedule')})
    }

    request.put(`Semester/${id}`, {
      Term: semester.Term, DateStart: semester.DateStart,
      DateEnd: semester.DateEnd, State: (semester.State - 1)
    }).then(res => {
        if (res.status === 200) {
          setIsConfirm(false)
          toast.success('Success to return previous state', {
            position: "top-right", autoClose: 2000, hideProgressBar: false, closeOnClick: true,
            pauseOnHover: true, draggable: true, progress: undefined, theme: "light",
          });
        }
      }).catch(err => {
        alert('Fail to return previous state')
        setIsConfirm(false)
      })
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack direction='row' justifyContent='space-between' mt={1} alignItems='center'>
        <Stack direction='row' alignItems='center' gap={4}>
          <Tooltip title='Back to Semester'>
            <IconButton onClick={backToSemester}>
              <ArrowBackIosNew />
            </IconButton>
          </Tooltip>
          <Title title={`Semester: ${semester.Term}`} />
        </Stack>
        <Stack pr={9} direction='row' gap={1}>
          {semester.State !== 1 && 
            <Button variant='outlined' color='info' size='small' onClick={clickPrevState}>
            Previous State</Button>}
          {semester.State !== 6 && 
            <Button variant='contained' color='success' size='small' onClick={clickNextState}>
            Next State</Button>}
        </Stack>
      </Stack>
      <Stack px={11} gap={1} mb={1}>
        <Typography><span style={{fontWeight: 500}}>Start:</span> {semester.DateStartFormat}</Typography>
        <Typography><span style={{fontWeight: 500}}>End:</span> {semester.DateEndFormat}</Typography>
        <Typography><span style={{fontWeight: 500}}>Status:</span> {semester.DateStatus}</Typography>
      </Stack>
      <Stack px={9} mb={2}>
        <Stack direction='row' gap={1} border='1px solid #e3e3e3' py={0.5} borderRadius={2}
          justifyContent='center' flexWrap='wrap'>
          {states.map(state => (
            <Stack key={state.id} direction='row' alignItems='center' gap={1}>
              <Stack width={40} height={40} borderRadius='50%' alignItems='center' justifyContent='center'
                bgcolor={semester.State >= state.id ? blue[600] : grey[300]}>
                {semester.State >= state.id && <Check sx={{ color: 'white' }} />}
              </Stack>
              <Typography>{state.name}</Typography>
              {state.id !== 6 && <HorizontalRule />}
            </Stack>
          ))}
        </Stack>
      </Stack>
      <Stack px={9} mb={2}>
        <Stack direction='row' gap={6} borderBottom='1px solid #e3e3e3'>
          <Typography color={selected === 'courses' ? green[600] : grey[500]} py={0.5}
            borderBottom={selected === 'courses' && `4px solid ${green[600]}`}
            fontSize='20px' onClick={() => setSelected('courses')}
            sx={{ '&:hover': { cursor: 'pointer', color: green[600] } }}>
            Courses
          </Typography>
          <Typography color={selected === 'subjects' ? green[600] : grey[500]} py={0.5}
            borderBottom={selected === 'subjects' && `4px solid ${green[600]}`}
            fontSize='20px' onClick={() => setSelected('subjects')}
            sx={{ '&:hover': { cursor: 'pointer', color: green[600] } }}>
            Subjects
          </Typography>
          <Typography color={selected === 'slot' ? green[600] : grey[500]} py={0.5}
            borderBottom={selected === 'slot' && `4px solid ${green[600]}`}
            fontSize='20px' onClick={() => setSelected('slot')}
            sx={{ '&:hover': { cursor: 'pointer', color: green[600] } }}>
            Slot Type
          </Typography>
          <Typography color={selected === 'lecturers' ? green[600] : grey[500]} py={0.5}
            borderBottom={selected === 'lecturers' && `4px solid ${green[600]}`}
            fontSize='20px' onClick={() => setSelected('lecturers')}
            sx={{ '&:hover': { cursor: 'pointer', color: green[600] } }}>
            Lecturers</Typography>
          {semester.State === 5 && 
          <Typography color={selected === 'confirm' ? green[600] : grey[500]} py={0.5}
            borderBottom={selected === 'confirm' && `4px solid ${green[600]}`}
            fontSize='20px' onClick={() => setSelected('confirm')}
            sx={{ '&:hover': { cursor: 'pointer', color: green[600] } }}>
            Confirmation</Typography>}
        </Stack>
      </Stack>
      {selected === 'courses' && <CourseList semesterId={id} scheduleId={schedule.Id} 
        slotTypes={slotTypes} semesterState={semester.State}/>}
      {selected === 'subjects' && <SummarySubject semesterId={id} scheduleId={schedule.Id}/>}
      {selected === 'slot' && <SlotType semesterId={id} />}
      {selected === 'lecturers' && <LecturerContainer semester={semester} admin={true} scheduleId={schedule.Id}/>}
      {selected === 'confirm' && <ViewConfirm semesterId={id} checkAllConfirm={checkAllConfirm}
        setReCheckAll={setReCheckAll}/>}
      <ConfirmModal isConfirm={isConfirm} setIsConfirm={setIsConfirm} content={content} 
        mode={mode} saveNextState={saveNextState} savePrevState={savePrevState} />
      <ToastContainer />
    </Stack>
  )
}

export default SemesterDetailAdmin

const states = [
  {id: 1, name: 'New'},
  {id: 2, name: 'Voting'},
  {id: 3, name: 'Evaluating'},
  {id: 4, name: 'Blocked'},
  {id: 5, name: 'Adjusting'},
  {id: 6, name: 'Public'},
]