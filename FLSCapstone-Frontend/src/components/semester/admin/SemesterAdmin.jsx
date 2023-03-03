import { Add } from '@mui/icons-material';
import { Button, Stack } from '@mui/material'
import { green } from '@mui/material/colors';
import React, { useEffect, useState } from 'react'
import { HashLoader } from 'react-spinners';
import { ToastContainer, toast } from 'react-toastify';
import request from '../../../utils/request';
import DeleteModal from '../../priority/DeleteModal';
import Title from '../../title/Title'
import SemesterCardAdmin from './SemesterCardAdmin';
import SemesterCreate from './SemesterCreate';
import SemesterUpdate from './SemesterUpdate';

const SemesterAdmin = () => {
  const [isCreate, setIsCreate] = useState(false);
  const [semesters, setSemesters] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedSemester, setSelectedSemester] = useState({});
  const [isDelete, setIsDelete] = useState(false);
  const [contentDelete, setContentDelete] = useState('');
  const [isUpdate, setIsUpdate] = useState(false);
  const [reload, setReload] = useState(false);

  useEffect(() => {
    setLoading(true)
    request.get('Semester', {
      params: {
        sortBy: 'DateEnd', order: 'Des',
        pageIndex: 1, pageSize: 100
      }
    })
      .then(res => {
        if (res.status === 200) {
          setSemesters(res.data)
          setLoading(false);
        }
      })
      .catch(err => {
        alert('Fail to load semesters!')
        setLoading(false)
      })
  }, [reload])

  const clickDelete = (pickedSemester) => {
    setSelectedSemester(pickedSemester);
    setContentDelete(`Semester: ${pickedSemester.Term}`)
    setIsDelete(true)
  }

  const saveDelete = () => {
    if(selectedSemester.Id){
      request.delete(`Semester/${selectedSemester.Id}`)
      .then(res => {
        if(res.status === 200){
          setIsDelete(false);
          setReload(!reload)
        }
      })
      .catch(err => {alert('Fail to delete')})
    }
  }

  const clickUpdate = (pickedSemester) => {
    setSelectedSemester(pickedSemester)
    setIsUpdate(true)
  }

  const handleAfterCreate = (status) => {
    if(status) {
      setReload(prev => !prev)
      toast.success('Create Successfully!', {
        position: "top-right", autoClose: 3000, hideProgressBar: false,
        closeOnClick: true, pauseOnHover: true, draggable: true,
        progress: undefined, theme: "light",
      });
    }
  }

  const handleAfterUpdate = (status) => {
    if(status) {
      setReload(prev => !prev)
      toast.success('Update Successfully!', {
        position: "top-right", autoClose: 3000, hideProgressBar: false,
        closeOnClick: true, pauseOnHover: true, draggable: true,
        progress: undefined, theme: "light",
      });
    }
  }

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Stack px={9} mt={1} mb={6} direction='row' justifyContent='space-between' alignItems='flex-start'>
        <Title title='Semester' subTitle='List of all semesters' />
        <Button variant='contained' color='success' onClick={() => setIsCreate(true)} endIcon={<Add/>}>
          Create</Button>
      </Stack>
      <Stack px={9} gap={4} direction='row' flexWrap='wrap' justifyContent='space-between'>
        {
          loading && <HashLoader size={30} color={green[600]} />
        }
        {
          !loading &&
          semesters.map(semester => (
            <SemesterCardAdmin key={semester.Id} semester={semester} 
              clickDelete={clickDelete} clickUpdate={clickUpdate}/>
          ))
        }
      </Stack>
      <SemesterCreate isCreate={isCreate} setIsCreate={setIsCreate} handleAfterCreate={handleAfterCreate}/>
      <SemesterUpdate isUpdate={isUpdate} setIsUpdate={setIsUpdate} 
        selectedSemester={selectedSemester} handleAfterUpdate={handleAfterUpdate}/>
      <DeleteModal isDelete={isDelete} setIsDelete={setIsDelete} saveDelete={saveDelete}
        contentDelete={contentDelete}/>
      <ToastContainer/>
    </Stack>
  )
}

export default SemesterAdmin