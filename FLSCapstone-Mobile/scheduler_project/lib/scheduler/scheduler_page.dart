// ignore_for_file: prefer_final_fields, prefer_const_literals_to_create_immutables, avoid_unnecessary_containers, unnecessary_null_comparison

import 'package:date_picker_timeline/date_picker_timeline.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:intl/intl.dart';
import 'package:scheduler_project/model/course_asign.dart';
import 'package:scheduler_project/model/scheduler.dart';
import 'package:scheduler_project/model/semester.dart';
import 'package:scheduler_project/model/slot_type.dart';
import 'package:scheduler_project/scheduler/components/list.dart';
import 'package:scheduler_project/services/api_handler.dart';

import '../model/scheduler_model.dart';
import '../model/user.dart';
import 'components/calendar_screen.dart';
import 'components/list_course.dart';

class SchedulerPage extends StatefulWidget {
  final UserModel userModel;
  const SchedulerPage({
    Key? key,
    required this.userModel,
  }) : super(key: key);

  @override
  State<SchedulerPage> createState() => _SchedulerPageState();
}

class _SchedulerPageState extends State<SchedulerPage> {
  DateTime _selectedDate = DateTime.now();
  //final DatePickerController _controller = DatePickerController();
  List<CourseAssignModel> listCourseAssign = [];
  List<SlotTypeModel> listSlotType = [];
  List<SemesterModel> listSemester = [];
  SemesterModel semester = SemesterModel();
  SemesterModel? _selectedSemester;
  SemesterModel semesterChose = SemesterModel();

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getSchedulerBySemes();
  }

  Future<void> getSchedulerBySemes() async {
    // ScheduleModel scheduleModel =
    //     await APIHandler.getSchedulerBySemesID(semesterId: 'FA22');
    // listCourseAssign = await APIHandler.getAllCourseAssignByLeSche(
    //     lecturerId: widget.userModel.id.toString(),
    //     schedulerId: scheduleModel.id.toString());
    listSlotType = await APIHandler.getAllSlotTypeAsc();
    listSemester = await APIHandler.getAllSemesterDec();
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    // WidgetsBinding.instance.addPostFrameCallback((_) => executeAfterBuild());
    if (_selectedSemester == null) {
      for (var se in listSemester) {
        if (se.dateStatus == 'On Going') {
          semesterChose = se;
        }
      }
    } else {
      semesterChose = _selectedSemester!;
    }
    return Scaffold(
      appBar: _appBar(),
      body: Column(
        children: [
          _taskAppBar(),
          //CalendarScreen(),
          //_addDateBar(),
          FutureBuilder<ScheduleModel>(
            future: APIHandler.getSchedulerBySemesID(
                semesterId: semesterChose.id.toString()),
            builder: ((context, snapshot) {
              if (snapshot.connectionState == ConnectionState.waiting) {
                return Center(
                  child: CircularProgressIndicator(),
                );
              } else if (snapshot.hasError) {
                return Center(
                  child: Text("An error occurred ${snapshot.error}"),
                );
              } else if (snapshot.data == null) {
                return Center(
                  child: Text("No schedule has been added yet"),
                );
              }
              return CalendarScreen(
                scheduleModel: snapshot.data!,
                userModel: widget.userModel,
                listSlotType: listSlotType,
                semesterChose: semesterChose,
              );
            }),
          ),
          // ListCourse(
          //     schedulerList:
          //         getCourseByMonday(DateFormat('EEEE').format(_selectedDate))),
        ],
      ),
    );
  }

  _appBar() {
    return AppBar(
      backgroundColor: Colors.green[700],
      elevation: 0,
      title: Text('Scheduler'),
    );
  }

  _taskAppBar() {
    return Container(
      margin: EdgeInsets.only(right: 20, left: 20, top: 10),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Container(
            //margin: EdgeInsets.symmetric(horizontal: 20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  DateFormat.yMMMMd().format(
                    _selectedDate,
                  ),
                  style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      fontFamily: GoogleFonts.lato().fontFamily,
                      color: Colors.grey),
                ),
                Text(
                  'Today',
                  style: TextStyle(
                      fontSize: 26,
                      fontWeight: FontWeight.bold,
                      fontFamily: GoogleFonts.lato().fontFamily,
                      color: Colors.black),
                ),
              ],
            ),
          ),
          Container(
            padding: EdgeInsets.only(left: 10),
            decoration: BoxDecoration(
              border: Border.all(width: 1, color: Colors.grey),
              borderRadius: BorderRadius.circular(10),
            ),
            margin: EdgeInsets.only(left: 10),
            child: DropdownButton<SemesterModel>(
              underline: Container(),
              //isExpanded: true,
              hint: Text(
                semesterChose.term.toString(),
                style: TextStyle(
                    color: Colors.orange.shade600,
                    fontSize: 18,
                    fontWeight: FontWeight.bold),
              ),
              value: _selectedSemester,
              onChanged: (newValue) {
                setState(() {
                  _selectedSemester = newValue;
                });
              },
              items: listSemester.map((SemesterModel value) {
                return DropdownMenuItem<SemesterModel>(
                  value: value,
                  child: Text(
                    value.term.toString(),
                    style: TextStyle(
                        fontSize: 18,
                        color: Colors.orange.shade600,
                        fontWeight: FontWeight.bold),
                  ),
                );
              }).toList(),
            ),
          ),
        ],
      ),
    );
  }

  // _addDateBar() {
  //   return Container(
  //     margin: EdgeInsets.only(top: 20, left: 5),
  //     child: DatePicker(
  //       DateTime(2022, 10, 01),
  //       height: 100,
  //       width: 50,
  //       initialSelectedDate: DateTime.now(),
  //       controller: _controller,
  //       selectionColor: Colors.orange.shade600,
  //       selectedTextColor: Colors.white,
  //       dateTextStyle: GoogleFonts.lato(
  //         textStyle: TextStyle(
  //             fontSize: 18, fontWeight: FontWeight.w600, color: Colors.grey),
  //       ),
  //       dayTextStyle: GoogleFonts.lato(
  //         textStyle: TextStyle(
  //             fontSize: 13, fontWeight: FontWeight.w600, color: Colors.grey),
  //       ),
  //       monthTextStyle: GoogleFonts.lato(
  //         textStyle: TextStyle(
  //             fontSize: 12, fontWeight: FontWeight.w600, color: Colors.grey),
  //       ),
  //       onDateChange: (date) {
  //         setState(() {
  //           _selectedDate = date;
  //         });
  //       },
  //     ),
  //   );
  // }

  // executeAfterBuild() {
  //   _controller.animateToSelection();
  // }

  getCourseByDay(String day) {
    print('day: $day');
    List<CourseAssignModel> course = [];
    List<SchedulerModel> scheduler = [];
    for (var i = 0; i < listSlotType.length; i++) {
      for (var j = 0; j < listCourseAssign.length; j++) {
        if (listCourseAssign[j].slotTypeId == listSlotType[i].id) {
          if (listSlotType[i].convertDateOfWeek.toString().split(' ')[0] ==
                  day ||
              listSlotType[i].convertDateOfWeek.toString().split(' ')[2] ==
                  day) {
            SchedulerModel schedulerModel = SchedulerModel(
              course: listCourseAssign[j].courseId.toString().split('_')[0],
              clas: listCourseAssign[j].courseId.toString().split('_')[1],
              slotTypeId: listSlotType[i].id.toString(),
              timeStart: listSlotType[i].timeStart.toString(),
              timeEnd: listSlotType[i].timeEnd.toString(),
              slotNumber: int.parse(listSlotType[i].slotNumber.toString()),
              duration: listSlotType[i].duration.toString(),
            );
            scheduler.add(schedulerModel);
            //course.add(listCourseAssign[j]);
          }
        }
      }
    }
    print('List courseAssign ${listCourseAssign.length}');
    print('List slotType ${listSlotType.length}');
    //print('Course ${course.length}');
    print('Scheduler: ${scheduler.length}');
    // for (var se in scheduler) {
    //   print("${se.course} - ${se.clas}, Slot: ${se.slotNumber}");
    // }
    return scheduler;
  }
}
