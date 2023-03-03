import { Cached, Close } from '@mui/icons-material';
import { Box, Button, Dialog, DialogContent, DialogTitle, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material'
import { grey } from '@mui/material/colors';
import React, { useEffect, useState } from 'react'
import { ClipLoader } from 'react-spinners';
import request from '../../utils/request';
import SwapConfirm from './SwapConfirm';

const SwapModal = ({ isSwapModal, setIsSwapModal, selectedSwap, handleAfterSwap }) => {
  const [blankSlots, setBlankSlots] = useState([]);
  const [swapCourses, setSwapCourses] = useState([]);
  const [isConfirm, setIsConfirm] = useState(false);
  const [alertConfirm, setAlertConfirm] = useState('');
  const [selected, setSelected] = useState({});
  const [loadingSwap, setLoadingSwap] = useState(false);

  useEffect(() => {
    if (selectedSwap.Id) {
      request.get(`Swap/CourseAssignToSwap/${selectedSwap.Id}`)
        .then(res => {
          if (res.data) setSwapCourses(res.data)
        })
        .catch(err => alert('Fail to get slots have course'))

      request.get(`Swap/BlankSlot/${selectedSwap.Id}`)
        .then(res => {
          if (res.data) setBlankSlots(res.data)
        })
        .catch(err => { alert('Fail to get empty slots') })
    }

  }, [selectedSwap.Id, isSwapModal])

  const clickEmptySlot = (picked) => {
    setSelected(picked)
    setAlertConfirm(`Sure to swap into this empty slot: ${picked.SlotTypeCode} ?`)
    setIsConfirm(true);
  }

  const clickAssignedSlot = (picked) => {
    setSelected(picked)
    setAlertConfirm(`This slot has assigned course: ${picked.CourseId}. \n Continue for swapping slot between 2 courses.`)
    setIsConfirm(true);
  }

  const handleSwap = () => {
    if(selected.Id){
      if(blankSlots.find(slot => slot.Id === selected.Id)){
        setIsConfirm(false)
        setLoadingSwap(true)
        request.put(`CourseAssign/${selectedSwap.Id}`, {
          LecturerId: selectedSwap.LecturerId, CourseId: selectedSwap.CourseId,
          SlotTypeId: selected.Id, ScheduleId: selectedSwap.ScheduleId,
          isAssign: selectedSwap.isAssign
        }).then(res => {
          if(res.status === 200){
            setIsSwapModal(false)
            setLoadingSwap(false)
            handleAfterSwap(true);
          }
        }).catch(err => {alert('Fail to swap into empty slot'); setLoadingSwap(false)})
      }

      if(swapCourses.find(course => course.Id === selected.Id)){
        setIsConfirm(false)
        setLoadingSwap(true)
        request.put(`CourseAssign/${selectedSwap.Id}`,{
          LecturerId: selectedSwap.LecturerId, CourseId: selectedSwap.CourseId,
          SlotTypeId: selected.SlotTypeId, ScheduleId: selectedSwap.ScheduleId,
          isAssign: selectedSwap.isAssign
        }).then(res => {
          if(res.status === 200){
            request.put(`CourseAssign/${selected.Id}`, {
              LecturerId: selected.LecturerId, CourseId: selected.CourseId,
              SlotTypeId: selectedSwap.SlotTypeId, ScheduleId: selected.ScheduleId,
              isAssign: selected.isAssign
            }).then(res => {
              if(res.status === 200){
                setIsSwapModal(false)
                setLoadingSwap(false)
                handleAfterSwap(true);
              }
            }).catch(err => {alert('Fail to swap between 2 courses!'); setLoadingSwap(false)})
          }
        }).catch(err => {alert('Fail to swap between 2 courses'); setLoadingSwap(false)})
      }
    }
  }

  return (
    <Dialog maxWidth='md' fullWidth={true}
      open={isSwapModal} onClose={() => setIsSwapModal(false)}>
      <DialogTitle>
        <Stack direction='row' alignItems='center' justifyContent='space-between'>
          <Typography variant='h5' fontWeight={500}>Swap course and slot</Typography>
          <Button size='small' variant='outlined' endIcon={<Close/>} color='error'
            onClick={() => setIsSwapModal(false)}>
            Close</Button>
        </Stack>
      </DialogTitle>
      <DialogContent>
        <Stack mb={2} gap={0.5}>
          <Typography>Course: {selectedSwap.CourseId}</Typography>
          <Typography>Slot: {selectedSwap.Duration} ({selectedSwap.ConvertDateOfWeek})</Typography>
        </Stack>
        <Stack>
          <Typography fontWeight={500}>Swapable slots</Typography>
          <Paper sx={{ minWidth: 700, mb: 2 }}>
            <TableContainer component={Box}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell size='small' className='subject-header'>Slot Code</TableCell>
                    <TableCell size='small' className='subject-header'>Slot Info</TableCell>
                    <TableCell size='small' className='subject-header'>Assigned Course</TableCell>
                    <TableCell size='small' className='subject-header'>Action</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {blankSlots.map(slot => (
                    <TableRow key={slot.Id}>
                      <TableCell size='small'>{slot.SlotTypeCode}</TableCell>
                      <TableCell size='small'>{slot.Duration}, {slot.ConvertDateOfWeek}</TableCell>
                      <TableCell size='small' sx={{color: grey[500]}}>No assigned course</TableCell>
                      <TableCell size='small'>
                        {loadingSwap ? (selected.Id === slot.Id ? 
                          <Button size='small' variant='contained'>
                            <ClipLoader size={20} color='white'/>
                          </Button> : 
                          <Button size='small' variant='contained' endIcon={<Cached/>} disabled>
                            Swap
                          </Button>) : 
                          <Button size='small' variant='contained' endIcon={<Cached/>}
                            onClick={() => clickEmptySlot(slot)} disabled={loadingSwap}>
                            Swap</Button>
                        }
                      </TableCell>
                    </TableRow>
                  ))}
                  {swapCourses.map(course => (
                    <TableRow key={course.Id}>
                      <TableCell size='small'>{course.SlotTypeCode}</TableCell>
                      <TableCell size='small'>{course.Duration}, {course.ConvertDateOfWeek}</TableCell>
                      <TableCell size='small'>{course.CourseId}</TableCell>
                      <TableCell size='small'>
                        {loadingSwap ? (selected.Id === course.Id ? 
                          <Button size='small' variant='contained'>
                            <ClipLoader size={20} color='white'/>
                          </Button> : 
                          <Button size='small' variant='contained' endIcon={<Cached/>} disabled>
                            Swap
                          </Button>) : 
                          <Button size='small' variant='contained' endIcon={<Cached/>}
                            onClick={() => clickAssignedSlot(course)} disabled={loadingSwap}>
                            Swap
                          </Button>
                        }
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Paper>
        </Stack>
      </DialogContent>
      <SwapConfirm isConfirm={isConfirm} setIsConfirm={setIsConfirm} 
        alertConfirm={alertConfirm} handleSwap={handleSwap}/>
    </Dialog>
  )
}

export default SwapModal