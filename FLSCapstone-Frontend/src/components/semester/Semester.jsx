import { Stack, Typography } from '@mui/material'
import { useState, useEffect } from 'react'
import './Semester.css'
import {HashLoader} from 'react-spinners';
import request from '../../utils/request';
import SemesterCard from './SemesterCard';
import { green } from '@mui/material/colors';

const Semester = () => {
  const [semesters, setSemesters] = useState([]);
  const [loading, setLoading] = useState(false);

  //get semester list
  useEffect(() => {
    setLoading(true)
    request.get('Semester', {
      params: {
        sortBy: 'DateEnd',
        order: 'Des',
        pageIndex: 1,
        pageSize: 100
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
  }, [])

  return (
    <Stack flex={5} height='90vh' overflow='auto'>
      <Typography variant='h5' color='#778899' fontWeight={500} px={9} mt={1}>
        Semester
      </Typography>
      <Typography color='gray' px={9} variant='subtitle1' mb={4}>
        List of all semesters
      </Typography>
      <Stack px={9} gap={4} direction='row' flexWrap='wrap' justifyContent='space-between'>
        {
          loading && <HashLoader size={30} color={green[600]}/>
        }
        {
          !loading &&
          semesters.map(semester => (
            <SemesterCard key={semester.Id} semester={semester} />
          ))
        }
      </Stack>
    </Stack>
  )
}

export default Semester
