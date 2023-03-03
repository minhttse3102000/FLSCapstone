import { ArrowBackIosNew } from '@mui/icons-material'
import { Box, IconButton, MenuItem, Select, Stack, Tooltip, Typography } from '@mui/material'
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import request from '../../utils/request';
import { getSemesterWeeks, getWeeksInYear } from '../../utils/weeksInYear';
import Timetable from '../main/Timetable';

const ScheduleDetail = ({admin}) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [lecturer, setLecturer] = useState({})
  const [semesters, setSemesters] = useState([])
  const [selectedSemesterObj, setSelectedSemesterObj] = useState({});
  const [selectedSemester, setSelectedSemester] = useState('');
  const [weeksInYear, setWeeksInYear] = useState(getWeeksInYear(new Date().getFullYear()));
  const [weeksInSemester, setWeeksInSemester] = useState([]);
  const [selectedWeek, setSelectedWeek] = useState('');
  const [selectedWeekObj, setSelectedWeekObj] = useState({});

  useEffect(() => {
    request.get(`User/${id}`)
    .then(res => {
      if(res.data){
        setLecturer(res.data)
      }
    })
    .catch(err => {
      alert('Fail to load lecturer!')
    })
  }, [id])

  useEffect(() => {
    const getSemesters = async() => {
      try {
        const response = await request.get('Semester', {
          params: {
            sortBy: 'DateEnd',
            order: 'Des',
            pageIndex: 1,
            pageSize: 999
          }
        })
        if(response.status === 200){
          setSemesters(response.data)
        }
      } 
      catch (error) {
        alert('Fail to load Semester!')
      }
    }

    getSemesters();
  }, [])

  useEffect(() => {
    if(semesters.length > 0){
      let state = false;
      const currentDate = new Date();
      for(let i in semesters){
        if(currentDate >= new Date(semesters[i].DateStartFormat) && 
            currentDate <= new Date(semesters[i].DateEndFormat))
        {
          state = true;
          setSelectedSemester(semesters[i].Id)
          setSelectedSemesterObj(semesters[i]);
          break;
        }
      }
      if(!state){
        setSelectedSemester(semesters[0].Id)
        setSelectedSemesterObj(semesters[0]);
      }
    }
  }, [semesters])

  useEffect(() => {
    if(Object.values(selectedSemesterObj).length > 0){
      const result = getSemesterWeeks(weeksInYear, selectedSemesterObj.DateStartFormat, selectedSemesterObj.DateEndFormat)
      setWeeksInSemester(result);
    }
  }, [selectedSemesterObj, weeksInYear])

  useEffect(() => {
    if(weeksInSemester.length > 0){
      const currentDay = new Date();
      let state = false;
      for(let i in weeksInSemester){
        const week = weeksInSemester[i].week;
        const start = new Date(week.split(' to ')[0]);
        start.setDate(start.getDate() - 1);
        const end = new Date(week.split(' to ')[1]);
        end.setDate(end.getDate() + 1)
        if(currentDay >= start && currentDay <= end){
          state=true;
          setSelectedWeek(weeksInSemester[i].id)
          setSelectedWeekObj(weeksInSemester[i])
          break;
        }
      }
      if(!state){
        setSelectedWeek(weeksInSemester[0].id)
        setSelectedWeekObj(weeksInSemester[0])
      }
    }
  }, [weeksInSemester])

  const handleSelectSemester = (e) => {
    setSelectedSemester(e.target.value)
    const selected = semesters.find(item => item.Id === e.target.value)
    setSelectedSemesterObj(selected)
    setWeeksInYear(getWeeksInYear(Number(selected.Term.split(' ')[1])))
  }

  const handleSelectWeek = (e) => {
    setSelectedWeek(e.target.value)
    setSelectedWeekObj(weeksInSemester.find(item => item.id === e.target.value))
  }

  const backToList = () => {
    if(admin){
      navigate(`/admin/schedule`)
    }
    else{
      navigate(`/manager/schedule`)
    }
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack direction='row' alignItems='center' color='#778899' gap={4} mt={1} mb={1}>
        <Tooltip title='Back to Lecturers'>
          <IconButton onClick={backToList}>
            <ArrowBackIosNew />
          </IconButton>
        </Tooltip>
        <Typography variant='h5' fontWeight={500}>
          Schedule: {lecturer.Name}
        </Typography>
      </Stack>
      <Stack direction='row' gap={8} px={9} mb={4}>
        <Typography>Name: {lecturer.Name}</Typography>
        <Typography>Email: {lecturer.Email}</Typography>
        <Typography>Department: {lecturer.DepartmentName}</Typography>
      </Stack>
      <Box height='100%' mb={1}>
        <Stack direction='row' gap={1} mb={1}>
          <Stack direction='row' alignItems='center' px={9} gap={1}>
            <Typography fontWeight={500}>Semester:</Typography>
            <Select color='success'
              size='small'
              value={selectedSemester}
              onChange={handleSelectSemester}
            >
              {
                semesters.map(semester => (
                  <MenuItem value={semester.Id} key={semester.Id}>
                    {semester.Term}
                  </MenuItem>
                ))
              }
            </Select>
          </Stack>
          <Stack direction='row' alignItems='center' px={9} gap={1}>
            <Typography fontWeight={500}>Week</Typography>
            <Select color='success'
              size='small'
              value={selectedWeek}
              onChange={handleSelectWeek}
            >
              {
                weeksInSemester.length > 0 &&
                weeksInSemester.map(week => (
                  <MenuItem value={week.id} key={week.id}>
                    <span>{week.week.split(' to ')[0].split('-')[2]}</span>
                    <span>/{week.week.split(' to ')[0].split('-')[1]}</span>
                    <span>{' - '}</span>
                    <span>{week.week.split(' to ')[1].split('-')[2]}</span>
                    <span>/{week.week.split(' to ')[1].split('-')[1]}</span>
                  </MenuItem>
                ))
              }
            </Select>
          </Stack>
        </Stack>

        <Timetable selectedSemester={selectedSemester} selectedWeekObj={selectedWeekObj} 
          lecturerId={id}/>
      </Box>
    </Stack>
  )
}

export default ScheduleDetail