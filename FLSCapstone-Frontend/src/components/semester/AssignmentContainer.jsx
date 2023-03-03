import { Assignment, Try } from '@mui/icons-material';
import { Button, Stack } from '@mui/material'
import { useState } from 'react'
import AssignmentList from '../assignment/AssignmentList';
import PriorityList from '../priority/PriorityList';

const AssignmentContainer = ({lecturer, semester, allSubjects, admin, myCourseGroup}) => {
  const [selected, setSelected] = useState('fixed');

  return (
    <Stack>
      <Stack direction='row' gap={2} mb={2}>
        <Button variant={selected === 'fixed' ? 'contained' : 'outlined'} sx={{textTransform: 'none'}} 
          startIcon={<Assignment/>} onClick={() => setSelected('fixed')}>
          Fixed Courses
        </Button>
        <Button variant={selected === 'priority' ? 'contained' : 'outlined'} sx={{textTransform: 'none'}} 
          startIcon={<Try/>} onClick={() => setSelected('priority')}>
          Priority Courses
        </Button>
      </Stack>
      {selected === 'fixed' && <AssignmentList lecturer={lecturer} semester={semester} 
        allSubjects={allSubjects} admin={admin} myCourseGroup={myCourseGroup}/>}
      {selected === 'priority' && <PriorityList lecturer={lecturer} semester={semester} 
          allSubjects={allSubjects} admin={admin}/>}
    </Stack>
  )
}

export default AssignmentContainer

