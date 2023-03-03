import { IconButton, Stack, Tooltip, Typography } from '@mui/material';
import { ArrowBackIosNew, Check, HorizontalRule } from '@mui/icons-material';
import React from 'react';
import { useState, useEffect } from 'react';
import SlotType from './SlotType';
import { useNavigate, useParams } from 'react-router-dom';
import Schedule from './Schedule';
import request from '../../utils/request';
import Title from '../title/Title';
import { blue, green, grey} from '@mui/material/colors';
import SubjectContainer from '../subject/SubjectContainer';

const SemesterDetail = () => {
  const [isSelected, setIsSelected] = useState(1)
  const { id } = useParams();
  const navigate = useNavigate();
  const [semester, setSemester] = useState({});

  //get semester by id
  useEffect(() => {
    request.get(`Semester/${id}`)
      .then(res => {
        if (res.status === 200) {
          setSemester(res.data)
        }
      })
      .catch(err => {
        alert('Fail to load Semester Detail!')
      })
  }, [id])

  const backToSemester = () => {
    navigate('/lecturer/semester')
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack direction='row' alignItems='center' justifyContent='space-between' mt={1}>
        <Stack direction='row' alignItems='center' gap={4}>
          <Tooltip title='Back to Semester' arrow>
            <IconButton onClick={backToSemester}>
              <ArrowBackIosNew />
            </IconButton>
          </Tooltip>
          <Title title={`Semester: ${semester.Term}`} />
        </Stack>
        <Stack pr={9} alignItems='center'>
          {semester.State === 1 &&
            <Typography bgcolor={blue[100]} p={1} borderRadius={2}>
              Semester has been created</Typography>}
          {semester.State === 2 &&
            <Stack bgcolor={blue[100]} p={0.5} alignItems='center' borderRadius={2}>
              <Typography>Voting is opened</Typography>
              <Typography>Rating subjects and slots now</Typography>
            </Stack>}
          {semester.State === 3 &&
            <Stack bgcolor={blue[100]} p={0.5} alignItems='center' borderRadius={2}>
              <Typography>Voting is closed</Typography>
              <Typography>Department Managers are evaluating</Typography>
            </Stack>}
          {semester.State === 4 &&
            <Stack bgcolor={blue[100]} p={0.5} alignItems='center' borderRadius={2}>
              <Typography>Semester has blocked</Typography>
              <Typography>Waiting for schedule generation</Typography>
            </Stack>}
          {semester.State === 5 &&
            <Stack bgcolor={blue[100]} p={0.5} alignItems='center' borderRadius={2}>
              <Typography>Schedule is generated</Typography>
              <Typography>You can preview the schedule</Typography>
            </Stack>}
          {semester.State === 6 &&
            <Typography bgcolor={blue[100]} p={1} borderRadius={2}>
              Schedule is public</Typography>}
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
        <Stack direction='row' gap={8} borderBottom='1px solid #e3e3e3'>
          {
            tabs.map(tab => (
              <Typography key={tab.id} fontSize='20px' py={0.5} onClick={() => setIsSelected(tab.id)}
                color={isSelected === tab.id ? green[700] : grey[500]}
                borderBottom={isSelected === tab.id && `4px solid ${green[700]}`}
                sx={{ '&:hover': { cursor: 'pointer', color: green[700] } }}>
                {tab.name}
              </Typography>
            ))
          }
        </Stack>
      </Stack>
      {
        isSelected === 1 &&
        <Schedule semester={semester}/>
      }
      {
        isSelected === 2 &&
        <SubjectContainer semesterId={id} semesterState={semester.State}/>
      }
      {
        isSelected === 3 &&
        <SlotType semesterId={id} semesterState={semester.State} />
      }
    </Stack>
  )
}

export default SemesterDetail

const tabs = [
  {
    id: 1,
    name: 'Schedule'
  },
  {
    id: 2,
    name: 'Subjects Rating'
  },
  {
    id: 3,
    name: 'Preference Slots'
  },
]

const states = [
  {id: 1, name: 'New'},
  {id: 2, name: 'Voting'},
  {id: 3, name: 'Evaluating'},
  {id: 4, name: 'Blocked'},
  {id: 5, name: 'Adjusting'},
  {id: 6, name: 'Public'},
]