import { Add, DeleteOutlined, EditOutlined } from '@mui/icons-material';
import {Box, Button, IconButton, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead,
  TablePagination, TableRow, Tooltip
} from '@mui/material';
import { green } from '@mui/material/colors';
import {HashLoader} from 'react-spinners'
import React, { useState, useEffect } from 'react'
import request from '../../utils/request';
import Title from '../title/Title'
import DepartmentCreate from './DepartmentCreate';
import DepartmentEdit from './DepartmentEdit';
import DeleteModal from '../priority/DeleteModal';

const DepartmentAdmin = () => {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [departments, setDepartments] = useState([]);
  const [managers, setManagers] = useState([]);
  const [isCreate, setIsCreate] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [pickedDepart, setPickedDepart] = useState({})
  const [isDelete, setIsDelete] = useState(false);
  const [contentDel, setContentDel] = useState('');
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    setLoading(true)
    request.get('Department', {
      params: {sortBy: 'Id', order: 'Asc',
        pageIndex: 1, pageSize: 1000
      }
    }).then(res => {
      if (res.data) {
        setDepartments(res.data);
        setLoading(false)
      }
    }).catch(err => {
      alert('Fail to load departments')
      setLoading(false)
    })
  }, [])

  useEffect(() => {
    request.get('User', {
      params:{RoleIDs: 'DMA',pageIndex: 1, pageSize: 100}
    }).then(res => {
      if(res.data){
        setManagers(res.data)
      }
    }).catch(err => {
      alert('Fail to load managers')
    })
  }, [])

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const clickEdit = (depart) => {
    setPickedDepart(depart)
    setIsEdit(true)
  }

  const clickDelete = (depart) =>{
    setPickedDepart(depart)
    setContentDel(`department: ${depart.Id}`)
    setIsDelete(true)
  }
  
  const saveDelete = () => {
    setIsDelete(false)
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack mt={1} mb={4} px={9}>
        <Title title='Department' subTitle='List of all departments' />
      </Stack>
      <Stack px={9} alignItems='flex-end' mb={1}>
        <Button variant='contained' endIcon={<Add/>} onClick={() => setIsCreate(true)}>
          Create
        </Button>
      </Stack>
      {loading && <Stack px={9}><HashLoader size={30} color={green[600]}/></Stack>}
      {!loading && <Stack px={9} mb={2}>
        <Paper sx={{ minWidth: 700 }}>
          <TableContainer component={Box}>
            <Table size='small'>
              <TableHead>
                <TableRow>
                  <TableCell className='subject-header'>Code</TableCell>
                  <TableCell className='subject-header'>Name</TableCell>
                  <TableCell className='subject-header'>Manager</TableCell>
                  <TableCell className='subject-header request-border'>Group</TableCell>
                  <TableCell className='subject-header' align='center'>Option</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {departments.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map(department => (
                    <TableRow key={department.Id} hover>
                      <TableCell>{department.Id}</TableCell>
                      <TableCell>{department.DepartmentName}</TableCell>
                      <TableCell>
                        {managers.find(manager => manager.DepartmentId === department.Id)?.Name}
                      </TableCell>
                      <TableCell className='request-border'>{department.DepartmentGroupId}</TableCell>
                      <TableCell align='center'>
                        <Tooltip title='Edit' placement='top' arrow>
                          <IconButton size='small' color='primary' onClick={() => clickEdit(department)}>
                            <EditOutlined/>
                          </IconButton>
                        </Tooltip>
                        <span>|</span>
                        <Tooltip title='Delete' placement='top' arrow>
                          <IconButton size='small' color='error' onClick={() => clickDelete(department)}>
                            <DeleteOutlined/>
                          </IconButton>
                        </Tooltip>
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </TableContainer>
          <TablePagination
            rowsPerPageOptions={[10, 20]}
            component='div'
            count={departments.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            showFirstButton
            showLastButton
            sx={{
              bgcolor: 'ghostwhite'
            }}
          />
        </Paper>
      </Stack>}
      <DepartmentCreate isCreate={isCreate} setIsCreate={setIsCreate}/>
      <DepartmentEdit isEdit={isEdit} setIsEdit={setIsEdit} pickedDepart={pickedDepart}/>
      <DeleteModal isDelete={isDelete} setIsDelete={setIsDelete} contentDelete={contentDel}
        saveDelete={saveDelete}/>
    </Stack>
  )
}

export default DepartmentAdmin