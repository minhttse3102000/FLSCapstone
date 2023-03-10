import { Box, Button, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, 
  TableRow, Typography } from '@mui/material'
import { ToastContainer, toast } from 'react-toastify';
import { useEffect, useMemo, useState } from 'react';
import request from '../../utils/request';
import GetCourseModal from './GetCourseModal';
import { grey } from '@mui/material/colors';

const GetCourse = ({semesterId, semesterState, lecturer, myCourseGroup}) => {
  const account = JSON.parse(localStorage.getItem('web-user'));
  const [scheduleId, setScheduleId] = useState('');
  const [slots, setSlots] = useState([]);
  const [myAssignCourses, setMyAssignCourses] = useState([]);
  const [isGet, setIsGet] = useState(false);
  const [lecturers, setLecturers] = useState([]);
  const [pickedSlot, setPickedSlot] = useState({});
  const [reload, setReload] = useState(false)
  const emptySlots = useMemo(() => {
    if(slots.length>0){
      let data = slots;
      for(let i in myAssignCourses){
        data = data.filter(item => item.Id !== myAssignCourses[i].SlotTypeId)
      }
      return data;
    }
    return []
  }, [slots, myAssignCourses])

  //get scheduleId
  useEffect(() => {
    if(semesterId){
      request.get('Schedule', {
        params: {SemesterId: semesterId, pageIndex: 1, pageSize: 1}
      }).then(res => {
        if(res.data.length > 0){
          setScheduleId(res.data[0]?.Id)
        }
      }).catch(err => {alert('Fail to get schedule of semester')})
    }
  }, [semesterId])

  //get lecturer's courses assign
  useEffect(() => {
    if(scheduleId && lecturer.Id){
      request.get('CourseAssign', {
        params: {LecturerId: lecturer.Id, ScheduleId: scheduleId, 
          sortBy: 'CourseId', order: 'Asc', pageIndex: 1, pageSize: 100
        }
      }).then(res => {
        if(res.data.length > 0){
          setMyAssignCourses(res.data)
        }
      }).catch(err => {alert('Fail to get assigned courses')})
    }
  }, [scheduleId, lecturer.Id, reload])

  //get slot type and filter with lecturer's courses to find empty slot
  useEffect(() => {
    if(semesterId){
      request.get('SlotType', {
        params: {SemesterId: semesterId, sortBy: 'DayOfWeekAndTimeStart', 
          order: 'Asc', pageIndex: 1, pageSize: 50,}
      }).then(res => {
        if(res.data.length > 0){
          setSlots(res.data)
        }
      }).catch(err => {alert('Fail to get empty slots')})
    }
  }, [semesterId])

  //get list lecturers inside depart
  useEffect(() => {
    if(lecturer.DepartmentId){
      request.get('User', {
        params: {DepartmentId: lecturer.DepartmentId, RoleIDs: 'LC', 
          pageIndex:1, pageSize:100}
      }).then(res => {
        if(res.data.length > 0){
          setLecturers(res.data)
        }
      }).catch(err => {alert('Fail to get inside lecturers')})
    }
  }, [lecturer.DepartmentId])

  const clickGetCourse = (slot) => {
    setPickedSlot(slot)
    setIsGet(true)
  }

  const afterGet = (status) => {
    if(status){
      setReload(pre => !pre)
      toast.success('Get course successfully!', {
        position: "top-right", autoClose: 3000, hideProgressBar: false,
        closeOnClick: true, pauseOnHover: true, draggable: true,
        progress: undefined, theme: "light",
      });
    }
  }

  return (
    <Stack height='90vh'>
      {semesterState === 5 && myCourseGroup.GroupName!=='confirm' && 
        lecturer.DepartmentId === account.DepartmentId && <>
      <Typography variant='subtitle1' color={grey[500]} mb={1}>
        *Add course into empty slots.The course will be taken from other internal lecturers.
      </Typography>
      <Typography fontWeight={500}>Empty Slots</Typography>
      <Paper sx={{ minWidth: 700, mb: 2 }}>
        <TableContainer component={Box}>
          <Table size='small'>
            <TableHead>
              <TableRow>
                <TableCell className='subject-header'>Slot Code</TableCell>
                <TableCell className='subject-header'>Slot Info</TableCell>
                <TableCell className='subject-header' align='center'>Slot Number</TableCell>
                <TableCell className='subject-header' align='center'>Action</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {emptySlots.map(slot => (
                <TableRow key={slot.Id} hover>
                  <TableCell>{slot.SlotTypeCode}</TableCell>
                  <TableCell>{slot.Duration}, {slot.ConvertDateOfWeek}</TableCell>
                  <TableCell align='center'>{slot.SlotNumber}</TableCell>
                  <TableCell align='center'>
                    <Button size='small' onClick={() => clickGetCourse(slot)}>
                      View Course</Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
      <GetCourseModal isGet={isGet} setIsGet={setIsGet} insideLecs={lecturers} 
        pickedSlot={pickedSlot} scheduleId={scheduleId} lecturer={lecturer} afterGet={afterGet}/>
      <ToastContainer/></>}
    </Stack>
  )
}

export default GetCourse