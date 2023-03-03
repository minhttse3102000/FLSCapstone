import { Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, Select, Stack, TextField, Typography } from '@mui/material'
import { red } from '@mui/material/colors'
import { useEffect, useState } from 'react'
import request from '../../utils/request';

const SubjectCreate = ({isCreate, setIsCreate}) => {
  const [departments, setDepartments] = useState([]);
  const [selectedDepart, setSelectedDepart] = useState('');

  useEffect(() => {
    request.get('Department', {
      params: {sortBy: 'Id', order:'Asc', pageIndex: 1, pageSize: 100}
    }).then(res => {
      if(res.status === 200){
        setDepartments(res.data)
        setSelectedDepart(res.data[0]?.Id)
      }
    }).catch(err => {alert('Fail to get departments')})
  }, [])

  return (
    <Dialog open={isCreate} onClose={() => setIsCreate(false)} fullWidth={true}>
      <DialogTitle variant='h5' fontWeight={500} mb={1}>
        Create Subject
      </DialogTitle>
      <DialogContent>
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
          <Typography fontWeight={500}>Code<span style={{color: red[600]}}>*</span></Typography>
          <TextField size='small'/>
        </Stack>
        <Stack mb={2}>
          <Typography fontWeight={500}>Name<span style={{color: red[600]}}>*</span></Typography>
          <TextField size='small'/>
        </Stack>
        <Stack mb={2}>
          <Typography fontWeight={500}>Description</Typography>
          <TextField size='small'/>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button color='info' variant='outlined' onClick={() => setIsCreate(false)}>Cancel</Button>
        <Button variant='contained' onClick={() => setIsCreate(false)}>Create</Button>
      </DialogActions>
    </Dialog>
  )
}

export default SubjectCreate