import { Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, Select, Stack, TextField, Typography } from '@mui/material'
import { red } from '@mui/material/colors'
import { useEffect, useState } from 'react'
import request from '../../utils/request';

const SubjectEdit = ({isEdit, setIsEdit, pickedSubject}) => {
  const [departments, setDepartments] = useState([]);
  const [selectedDepart, setSelectedDepart] = useState('');
  const [subName, setSubName] = useState('');
  const [subDes, setSubDes] = useState('');

  useEffect(() => {
    if(pickedSubject.Id){
      setSelectedDepart(pickedSubject.DepartmentId)
      setSubName(pickedSubject.SubjectName)
      setSubDes(pickedSubject.Description || '')
    }
  }, [pickedSubject, isEdit])

  useEffect(() => {
    request.get('Department', {
      params: {sortBy: 'Id', order:'Asc', pageIndex: 1, pageSize: 100}
    }).then(res => {
      if(res.status === 200){
        setDepartments(res.data)
      }
    }).catch(err => {alert('Fail to get departments')})
  }, [])

  return (
    <Dialog open={isEdit} onClose={() => setIsEdit(false)} fullWidth={true}>
      <DialogTitle variant='h5' fontWeight={500} mb={1}>
        Edit Subject
      </DialogTitle>
      <DialogContent>
        <Stack mb={2} direction='row' gap={1}>
          <Typography fontWeight={500}>Code: </Typography>
          <Typography>{pickedSubject.Id}</Typography>
        </Stack>
        <Stack mb={2}>
          <Typography fontWeight={500}>Department</Typography>
          <Select size='small' value={selectedDepart} 
            onChange={(e) => setSelectedDepart(e.target.value)}>
            {departments.map(depart => (
              <MenuItem key={depart.Id} value={depart.Id}>{depart.Id} - {depart.DepartmentName}</MenuItem>
            ))}
          </Select>
        </Stack>
        <Stack mb={2}>
          <Typography fontWeight={500}>Name<span style={{color: red[600]}}>*</span></Typography>
          <TextField size='small' value={subName} onChange={(e) => setSubName(e.target.value)}/>
        </Stack>
        <Stack mb={2}>
          <Typography fontWeight={500}>Description</Typography>
          <TextField size='small' value={subDes} onChange={(e) => setSubDes(e.target.value)}/>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button color='info' variant='outlined' onClick={() => setIsEdit(false)}>Cancel</Button>
        <Button variant='contained' onClick={() => setIsEdit(false)}>Edit</Button>
      </DialogActions>
    </Dialog>
  )
}

export default SubjectEdit