import { Box, Button, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material'
import { grey } from '@mui/material/colors'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CountUp from 'react-countup'
import request from '../../utils/request'
import './Home.css'

const ManagerHome = ({admin}) => {
  const navigate = useNavigate();
  const [semester, setSemester] = useState({});
  const [departments, setDepartments] = useState([]);
  const [subjects, setSubjects] = useState([]);
  const [lecturers, setLecturers] = useState([]);

  useEffect(() => {
    request.get('Semester', {
      params: {
        sortBy: 'DateEnd', order: 'Des',
        pageIndex: 1, pageSize: 100
      }
    })
      .then(res => {
        if (res.data.length > 0) {
          const data = res.data
          for(let i in data){
            if(data[i].DateStatus.toLowerCase() === 'on going'){
              setSemester(data[i]);
              break;
            }
          }
        }
      })
      .catch(err => {
        alert('Fail to load semesters!')
      })
  }, [])

  useEffect(() => {
    request.get('Department', {
      params: {
        sortBy: 'Id', order: 'Asc', pageIndex: 1, pageSize: 100
      }
    })
    .then(res => {
      if(res.data){
        setDepartments(res.data);
      }
    })
    .catch(err => alert('Fail to load departments'))
  }, [])

  useEffect(() => {
    request.get('Subject', {
      params: {
        sortBy: 'Id', order: 'Asc', pageIndex: 1, pageSize: 1000
      }
    })
    .then(res => {
      if(res.data){
        setSubjects(res.data);
      }
    })
    .catch(err => alert('Fail to load subjects'))
  }, [])

  useEffect(() => {
    request.get('User', {
      params: {
        RoleIDs: 'LC', sortBy: 'Name', order: 'Asc', pageIndex: 1, pageSize: 1000
      }
    })
    .then(res => {
      if(res.data){
        setLecturers(res.data);
      }
    })
    .catch(err => alert('Fail to load lecturers'))
  }, [])

  const toSemester = () => {
    if(admin){navigate('/admin/semester')}
    else{navigate('/manager/semester')}
  }

  const toDepartment = () => {
    if(admin) navigate('/admin/department')
    else navigate('/manager/department')
  }

  const toSubject = () => {
    if(admin) navigate('/admin/subject')
    else navigate('/manager/subject')
  }

  const toLecturer = () => {
    if(admin) navigate('/admin/lecturer')
    else navigate('/manager/lecturer')
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Typography px={9} variant='h5' mt={1} mb={6}>
        Welcome {admin ? 'Admin' : 'Department Manager'}
      </Typography>
      <Stack direction='row' px={9} gap={2} flexWrap='wrap' mb={4}>
        <Stack flex={1} className='summary-box' alignItems='center'>
          <Typography mt={1} color={grey[700]}>Current Semester</Typography>
          <Typography variant='h5' my={2}>{semester.Term}</Typography>
          <Button variant='contained' size='small' color='success' sx={{mb: 1}}
            onClick={toSemester}>
            Go to Semester
          </Button>
        </Stack>
        <Stack flex={1} className='summary-box' alignItems='center'>
          <Typography mt={1} color={grey[700]}>Total Departments</Typography>
          <Typography variant='h5' my={2}>
            <CountUp end={departments.length || 0} duration={0.5}/>
          </Typography>
          <Button variant='contained' size='small' color='success' sx={{mb: 1}}
            onClick={toDepartment}>
            Go to Department</Button>
        </Stack>
        <Stack flex={1} className='summary-box' alignItems='center'>
          <Typography mt={1} color={grey[700]}>Total Subjects</Typography>
          <Typography variant='h5' my={2}>
            <CountUp end={subjects.length || 0} duration={1}/>
          </Typography>
          <Button variant='contained' size='small' color='success' sx={{mb: 1}}
            onClick={toSubject}>
            Go to Subject</Button>
        </Stack>
        <Stack flex={1} className='summary-box' alignItems='center'>
          <Typography mt={1} color={grey[700]}>Total Lecturers</Typography>
          <Typography variant='h5' my={2}>
            <CountUp end={lecturers.length || 0} duration={1}/>
          </Typography>
          <Button variant='contained' size='small' color='success' sx={{mb: 1}}
            onClick={toLecturer}>
            Go to Lecturer</Button>
        </Stack>
      </Stack>
      <Stack px={9}>
        <Paper sx={{ minWidth: 700, mb: 2 }}>
          <TableContainer component={Box}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell size='small' className='subject-header'>Departments</TableCell>
                  <TableCell size='small' className='subject-header'>Total Subjects</TableCell>
                  <TableCell size='small' className='subject-header'>Total Lecturers</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
               {departments.map(department => (
                <TableRow key={department.Id} hover>
                  <TableCell size='small'>{department.DepartmentName}</TableCell>
                  <TableCell size='small'>
                    {subjects.filter(subject => subject.DepartmentId === department.Id).length}
                  </TableCell>
                  <TableCell size='small'>
                    {lecturers.filter(lecturer => lecturer.DepartmentId === department.Id).length}
                  </TableCell>
                </TableRow>
               ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      </Stack>
    </Stack>
  )
}

export default ManagerHome