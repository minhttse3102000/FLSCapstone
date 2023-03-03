import { CancelOutlined, ChatOutlined, EditOutlined } from '@mui/icons-material';
import {
  Alert, Box, IconButton, Paper, Stack, Switch, Table, TableBody, TableCell, TableContainer,
  TableHead, TablePagination, TableRow, Tooltip, Typography
} from '@mui/material'
import { blue, grey, red } from '@mui/material/colors';
import React, { useEffect, useMemo, useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import FeedbackModal from './FeedbackModal';
import MaxCourseModal from './MaxCourseModal';
import request from '../../utils/request';
import configData from '../../utils/configData.json'
import { ClipLoader } from 'react-spinners';

const FeedbackSelection = ({ id, semester, admin }) => {
  const account = JSON.parse(localStorage.getItem('web-user'));
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [isFeedback, setIsFeedback] = useState(false);
  const [isMax, setIsMax] = useState(false);
  const [lecturer, setLecturer] = useState({});
  const [subjects, setSubjects] = useState([]);
  const [points, setPoints] = useState([]);
  const [selectedId, setSelectedId] = useState('');
  const [loadPoint, setLoadPoint] = useState(false);
  const [isDisable, setIsDisable] = useState(false);
  const [loadDisable, setLoadDisable] = useState(false);
  const [afterDisable, setAfterDisable] = useState(false);

  const disableNumber = useMemo(() => {
    if (points.length > 0) {
      return points.filter(item => item.isEnable === 0).length
    }
    return 0;
  }, [points])

  //get lecturer by id
  useEffect(() => {
    if(id){
      request.get(`User/${id}`)
      .then(res => {
        if (res.data) {
          setLecturer(res.data)
        }
      })
      .catch(err => {
        alert('Fail to load lecturer in Schedule')
      })
    }
  }, [id])

  //get subject by department of lecturer
  useEffect(() => {
    const getSubjects = async () => {
      if (lecturer.DepartmentId) {
        try {
          const response = await request.get('Subject', {
            params: {
              DepartmentId: lecturer.DepartmentId, sortBy: 'Id', order: 'Asc',
              pageIndex: 1, pageSize: 1000
            }
          })
          if (response.data) {
            setSubjects(response.data)
          }
        }
        catch (error) { alert('Fail to load subjects!') }
      }
    }

    getSubjects();
  }, [lecturer.DepartmentId])

  //get list subject of lecturer to get favorite and feedback point
  useEffect(() => {
    const getFavoriteSubjects = async () => {
      try {
        const response = await request.get('SubjectOfLecturer', {
          params: {
            SemesterId: semester.Id, LecturerId: id,
            pageIndex: 1, pageSize: 1000
          }
        })
        if (response.data) {
          setPoints(response.data)
        }
      }
      catch (error) { alert('Fail to load favortite points') }
    }

    getFavoriteSubjects();
  }, [id, semester.Id, isFeedback, isMax, afterDisable])

  // set row per page
  useEffect(() => {
    if (subjects.length > 0) {
      setRowsPerPage(subjects.length)
    }
  }, [subjects])

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleFeedback = (subjectId) => {
    setSelectedId(subjectId);
    setLoadPoint(!loadPoint)
    setIsFeedback(true);
  }

  const handleEditMax = (subjectId) => {
    setSelectedId(subjectId)
    setLoadPoint(!loadPoint)
    setIsMax(true)
  }

  const handleDisable = (subjectId) => {
    if (!isDisable) return;

    if (points.length > 0) {
      const obj = points.find(item => item.SubjectId === subjectId);
      if (obj) {
        if (obj.isEnable === 1) {
          if (disableNumber >= configData.DISABLE_SUBJECT) return;
        }
        setSelectedId(subjectId)
        setLoadDisable(true)
        request.put(`SubjectOfLecturer/${obj.Id}`, {
          DepartmentManagerId: account.Id, SemesterId: obj.SemesterId,
          SubjectId: obj.SubjectId, LecturerId: obj.LecturerId,
          FavoritePoint: obj.FavoritePoint, FeedbackPoint: obj.FeedbackPoint,
          MaxCourseSubject: obj.MaxCourseSubject,
          isEnable: obj.isEnable === 1 ? 0 : 1
        }).then(res => {
          if (res.status === 200) {
            setAfterDisable(!afterDisable)
            setLoadDisable(false)
            toast.success('Update Successfully!', {
              position: "top-right", autoClose: 3000, hideProgressBar: false,
              closeOnClick: true, pauseOnHover: true, draggable: true,
              progress: undefined, theme: "light",
            });
          }
        }).catch(err => { alert('Fail to disable subject'); setLoadDisable(false) })
      }
    }
  }

  return (
    <Stack flex={5} height='90vh'>
      {!admin && lecturer.DepartmentId && lecturer.DepartmentId !== account.DepartmentId &&
        <Alert severity="error">Can not evaluate to the external lecturer.</Alert>
      }
      {((lecturer.DepartmentId && lecturer.DepartmentId === account.DepartmentId) || admin) && <>
        <Stack direction='row' alignItems='center' justifyContent='space-between' mb={1}>
          <Stack>
            <Typography color='gray' variant='subtitle1'>
              *Manager can evaluate lecturer's skill with each subject.
              {' '}All parameters is relative to generate schedule.
            </Typography>
            <Typography>Total: {subjects.length}</Typography>
          </Stack>
          {semester.State === 3 && !admin && 
          <Stack direction='row' alignItems='center' bgcolor={grey[200]}>
            <Switch checked={isDisable} onChange={() => setIsDisable(!isDisable)} />
            <Typography pr={2}>
              {isDisable ? <span style={{ color: blue[600] }}>Disable On</span> : 'Disable Off'}
            </Typography>
          </Stack>}
        </Stack>
        <Stack mb={2}>
          <Paper sx={{ minWidth: 700, mb: 2 }}>
            <TableContainer component={Box} sx={{ overflow: 'auto' }}>
              <Table size='small'>
                <TableHead>
                  <TableRow>
                    <TableCell className='subject-header' sx={{ borderRight: '1px solid #e3e3e3' }}>
                      Subject</TableCell>
                    <TableCell className='subject-header' align='center'
                      sx={{ borderRight: '1px solid #e3e3e3' }}>
                      <Tooltip title='This is graded by Lecturer' placement='top' arrow>
                        <span>Favorite</span>
                      </Tooltip>
                    </TableCell>
                    <TableCell className='subject-header' align='center'
                      sx={{ borderRight: '1px solid #e3e3e3' }}>
                      <Tooltip title="Evaluate by Lecturer's skill" placement='top' arrow>
                        <span>Evaluation</span>
                      </Tooltip>
                    </TableCell>
                    <TableCell className='subject-header' align='center'
                      sx={{ borderRight: '1px solid #e3e3e3' }}>
                      <Tooltip title='Edit max course number of each subject for lecturer' placement='top' arrow>
                        <span>Max Courses</span>
                      </Tooltip>
                    </TableCell>
                    <TableCell className='subject-header' align='center'>
                      <Tooltip title="Lecturer can't teach disable subjects" placement='top' arrow>
                        <span>Disable ({disableNumber}/{configData.DISABLE_SUBJECT})</span>
                      </Tooltip>
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {subjects.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                    .map((subject) => (
                      <TableRow key={subject.Id} hover>
                        <TableCell sx={{ borderRight: '1px solid #e3e3e3' }}>{subject.Id} - {subject.SubjectName}</TableCell>
                        <TableCell sx={{ borderRight: '1px solid #e3e3e3' }} align='center'>
                          <Typography fontSize='15px'>
                            {points.length > 0 && points.find(item => item.SubjectId === subject.Id)?.FavoritePoint}
                          </Typography>
                        </TableCell>
                        <TableCell sx={{ borderRight: '1px solid #e3e3e3' }}>
                          <Stack direction='row' alignItems='center' justifyContent='center'>
                            <Typography fontSize='15px' borderRight={semester.State === 3 && !admin && '1px solid gray'} pr={1}>
                              {points.find(item => item.SubjectId === subject.Id)?.FeedbackPoint === 1 && 'Weak'}
                              {points.find(item => item.SubjectId === subject.Id)?.FeedbackPoint === 2 && 'Not good'}
                              {points.find(item => item.SubjectId === subject.Id)?.FeedbackPoint === 3 && 'Normal'}
                              {points.find(item => item.SubjectId === subject.Id)?.FeedbackPoint === 4 && 'Good'}
                              {points.find(item => item.SubjectId === subject.Id)?.FeedbackPoint === 5 && 'Excellent'}
                            </Typography>
                            {semester.State === 3 && !admin &&
                              <Tooltip title='Evaluate' placement='right'>
                                <IconButton color='primary' onClick={() => handleFeedback(subject.Id)}
                                  size='small'>
                                  <ChatOutlined />
                                </IconButton>
                              </Tooltip>}
                          </Stack>
                        </TableCell>
                        <TableCell sx={{ borderRight: '1px solid #e3e3e3' }}>
                          <Stack direction='row' alignItems='center' gap={1} justifyContent='center'>
                            <Typography fontSize='15px' borderRight={semester.State === 3 && !admin && '1px solid gray'} pr={2}>
                              {points.length > 0 && points.find(item => item.SubjectId === subject.Id)?.MaxCourseSubject}
                            </Typography>
                            {semester.State === 3 && !admin &&
                              <Tooltip title='Edit' placement='right'>
                                <IconButton color='primary' onClick={() => handleEditMax(subject.Id)}
                                  size='small'>
                                  <EditOutlined />
                                </IconButton>
                              </Tooltip>}
                          </Stack>
                        </TableCell>
                        <TableCell align='center'
                          sx={{
                            bgcolor: points.find(item => item.SubjectId === subject.Id)?.isEnable === 0 ? '' : (disableNumber >= configData.DISABLE_SUBJECT ? grey[100] : ''),
                            '&:hover': { cursor: isDisable ? 'pointer' : 'default' }
                          }}
                          onClick={() => handleDisable(subject.Id)}>
                          {points.find(item => item.SubjectId === subject.Id)?.isEnable === 0 &&
                            <CancelOutlined sx={{ color: red[600] }} />
                          }
                          {loadDisable && selectedId === subject.Id && <ClipLoader size={20} color={grey[400]} />}
                        </TableCell>
                      </TableRow>))}
                </TableBody>
              </Table>
            </TableContainer>
            <TablePagination
              rowsPerPageOptions={[10, subjects.length]}
              component='div'
              count={subjects.length}
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
        </Stack> </>}
      <FeedbackModal isFeedback={isFeedback} setIsFeedback={setIsFeedback}
        lecturer={lecturer} subjectId={selectedId} points={points} loadPoint={loadPoint} />
      <MaxCourseModal isMax={isMax} setIsMax={setIsMax} lecturer={lecturer} subjectId={selectedId}
        points={points} loadPoint={loadPoint} />
      <ToastContainer />
    </Stack>
  )
}

export default FeedbackSelection