import { Alert, Button, Stack, TextField, Typography } from '@mui/material'
import { useEffect, useState } from 'react';
import { ClipLoader } from 'react-spinners';
import { ToastContainer, toast } from 'react-toastify';
import request from '../../utils/request';

const CourseNumber = ({lecturer, semesterId, semesterState, admin}) => {
  const account = JSON.parse(localStorage.getItem('web-user'));
  const [min, setMin] = useState(0);
  const [max, setMax] = useState(0);
  const [group, setGroup] = useState({});
  const [errorMin, setErrorMin] = useState('');
  const [errorMax, setErrorMax] = useState('');
  const [reload, setReload] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(false);
    request.get('LecturerCourseGroup', {
      params: {LecturerId: lecturer.Id, SemesterId: semesterId, 
        pageIndex: 1, pageSize: 1}
    }).then(res => {
      if(res.data){
        const data = res.data[0];
        setGroup(data);
        setMin(data.MinCourse);
        setMax(data.MaxCourse);
      }
    }).catch(err => {alert('Fail to get min and max course number')})
  }, [lecturer.Id, semesterId, reload])

  const handleChangeMin = (e) => {
    setErrorMin('');
    const value = e.target.value
    if(value === '') setMin('')
    else setMin(Number(value))

    if(Number(value) <= 0) setErrorMin('Min must be greater than 0')
    else if(Number(value) >= max) setErrorMin('Min must be less than Max') 
  }

  const handleChangeMax = (e) => {
    setErrorMax('')
    const value = e.target.value
    if(value === '') setMax('')
    else setMax(Number(value))

    if(Number(value) <= min) setErrorMax('Max must be greater than Min')
    else if(Number(value) > 12) setErrorMax('Maximum is 12')
  }

  const handleSave = () => {
    if(!(errorMin || errorMax || (group.MinCourse === min && group.MaxCourse === max))){
      setLoading(true);
      request.put(`LecturerCourseGroup/${group.Id}`, {
        LecturerId: lecturer.Id, SemesterId: semesterId,
        GroupName: group.GroupName, MinCourse: min, MaxCourse: max
      }).then(res => {
        if(res.status === 200){
          setReload(!reload)
          setLoading(false);
          toast.success('Save Successfully!', {
            position: "top-right", autoClose: 3000, hideProgressBar: false,
            closeOnClick: true, pauseOnHover: true, draggable: true,
            progress: undefined, theme: "light",
          });
        }
      }).catch(err => {alert('Fail to edit min max number'); setLoading(false);})
    }
  }

  return (
    <Stack height='90vh' alignItems='center'>
      {(admin || lecturer.DepartmentId === account.DepartmentId) && 
        <Typography variant='subtitle1' color='gray' mb={4}>
          *Edit min and max course number
        </Typography>}
      {!admin && lecturer.DepartmentId !== account.DepartmentId &&
        <Alert severity="error">Can not view of information of the external lecturer.</Alert>
      }
      {(admin || (lecturer.DepartmentId === account.DepartmentId)) && group.Id && <>
      <Stack direction='row' alignItems='center' gap={1} mb={2}>
        <Typography fontWeight={500} width={50}>Min:</Typography>
        <TextField size='small' type='number' color='success' error={errorMin ? true : false} 
          helperText={errorMin} value={min} onChange={handleChangeMin} 
          InputProps={{ readOnly: (admin || semesterState !== 3) ? true : false }}/>
      </Stack>
      <Stack direction='row' alignItems='center' gap={1} mb={2}>
        <Typography fontWeight={500} width={50}>Max:</Typography>
        <TextField size='small' type='number' color='success' error={errorMax ? true : false} 
          helperText={errorMax} value={max} onChange={handleChangeMax} 
          InputProps={{ readOnly: (admin || semesterState !== 3) ? true : false }}/>
      </Stack>
      {loading && <Button variant='contained' size='small' color='success'>
          <ClipLoader size={20} color='white'/>
        </Button>}
      {!admin && semesterState===3 && !loading && 
        <Button variant='contained' size='small' color='success' onClick={handleSave}
          disabled={(errorMin || errorMax || (group.MinCourse === min && group.MaxCourse === max)) ? true : false}>
          Save
        </Button>}
      <ToastContainer /></>}
    </Stack>
  )
}

export default CourseNumber
