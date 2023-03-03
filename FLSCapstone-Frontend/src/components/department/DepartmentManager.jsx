import { Check } from '@mui/icons-material';
import { Box, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip, Typography } from '@mui/material'
import { green } from '@mui/material/colors';
import { useEffect, useState } from 'react'
import request from '../../utils/request';
import Title from '../title/Title'

const DepartmentManager = () => {
  const account = JSON.parse(localStorage.getItem('web-user'));
  const [departments, setDepartments] = useState([]);
  const [managers, setManagers] = useState([]);

  useEffect(() => {
    request.get('Department', {
      params: {
        sortBy: 'Id', order: 'Asc',
        pageIndex: 1, pageSize: 1000
      }
    })
      .then(res => {
        if (res.data) {
          setDepartments(res.data);
        }
      })
      .catch(err => {
        alert('Fail to load departments')
      })
  }, [])

  useEffect(() => {
    request.get('User', {
      params:{RoleIDs: 'DMA',pageIndex: 1, pageSize: 100}
    })
    .then(res => {
      if(res.data){
        setManagers(res.data)
      }
    })
    .catch(err => {
      alert('Fail to load managers')
    })
  }, [])


  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack mt={1} mb={4} px={9}>
        <Title title='Department' subTitle='List of all departments' />
      </Stack>
      <Stack px={9} mb={2}>
        <Paper sx={{ minWidth: 700 }}>
          <TableContainer component={Box}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell size='small' className='subject-header'>Code</TableCell>
                  <TableCell size='small' className='subject-header'>Name</TableCell>
                  <TableCell size='small' className='subject-header'>Manager</TableCell>
                  <TableCell size='small' className='subject-header'>Group</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {departments.map(department => (
                    <TableRow key={department.Id} hover>
                      <TableCell size='small'>{department.Id}</TableCell>
                      <TableCell size='small'>
                        <Stack direction='row' alignItems='center' gap={1}>
                          <Typography fontSize='14px'>{department.DepartmentName}</Typography>
                          {account.DepartmentId === department.Id && 
                            <Tooltip title='My Department'>
                              <Check sx={{color: green[600]}}/>
                            </Tooltip>}
                        </Stack>
                      </TableCell>
                      <TableCell size='small'>
                        {managers.find(manager => manager.DepartmentId === department.Id)?.Name}
                      </TableCell>
                      <TableCell size='small'>{department.DepartmentGroupId}</TableCell>
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

export default DepartmentManager