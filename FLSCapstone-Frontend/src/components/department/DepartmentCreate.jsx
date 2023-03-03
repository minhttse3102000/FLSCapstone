import { Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, Select, Stack, TextField, Typography } from '@mui/material'
import { red } from '@mui/material/colors'
import { useEffect, useState } from 'react';
import request from '../../utils/request';

const DepartmentCreate = ({isCreate, setIsCreate}) => {
  const [groups, setGroups] = useState([]);
  const [selectedGroup, setSelectedGroup] = useState('');

  useEffect(() => {
    request.get('DepartmentGroup', {
      params: {pageIndex: 1, pageSize: 100}
    }).then(res => {
      if(res.data.length > 0){
        setGroups(res.data)
        setSelectedGroup(res.data[0]?.Id)
      }
    }).catch(err => {alert('Fail to get department groups')})
  }, [])

  return (
    <Dialog open={isCreate} onClose={() => setIsCreate(false)} fullWidth={true}>
      <DialogTitle variant='h5' fontWeight={500} mb={1}>
        Create Department
      </DialogTitle>
      <DialogContent>
        <Stack mb={2}>
          <Typography fontWeight={500}>Group</Typography>
          <Select size='small' value={selectedGroup} 
            onChange={(e) => setSelectedGroup(e.target.value)}>
            {groups.map(group => (
              <MenuItem key={group.Id} value={group.Id}>{group.Id} - {group.DepartmentGroupName}</MenuItem>
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
      </DialogContent>
      <DialogActions>
        <Button color='info' variant='outlined' onClick={() => setIsCreate(false)}>Cancel</Button>
        <Button variant='contained' onClick={() => setIsCreate(false)}>Create</Button>
      </DialogActions>
    </Dialog>
  )
}

export default DepartmentCreate