// ignore_for_file: prefer_const_constructors, avoid_print

import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:intl/intl.dart';
import 'package:table_calendar/table_calendar.dart';

import '../../model/course_asign.dart';
import '../../model/scheduler.dart';
import '../../model/scheduler_model.dart';
import '../../model/semester.dart';
import '../../model/slot_type.dart';
import '../../model/user.dart';
import '../../services/api_handler.dart';
import 'list_course.dart';

var now = DateTime.now();
var firstDay = DateTime(now.year, now.month - 13, now.day);
var lastDay = DateTime(now.year, now.month + 13, now.day);
var _selectedDay = DateTime.now();
var _focusedDay = DateTime.now();
List<DateTime> listTime = [];
CalendarFormat format = CalendarFormat.week;

class CalendarScreen extends StatefulWidget {
  CalendarScreen({
    Key? key,
    required this.scheduleModel,
    required this.userModel,
    required this.listSlotType,
    required this.semesterChose,
  }) : super(key: key);
  final ScheduleModel scheduleModel;
  final UserModel userModel;
  final List<SlotTypeModel> listSlotType;
  SemesterModel semesterChose;

  @override
  State<CalendarScreen> createState() => _CalendarScreenState();
}

class _CalendarScreenState extends State<CalendarScreen> {
  List<CourseAssignModel> listCourseAssign = [];

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    listTime = getCurrentWeek(_focusedDay);
    getSchedulerBySemes();
  }

  Future<void> getSchedulerBySemes() async {
    listCourseAssign = await APIHandler.getAllCourseAssignByLeSche(
        lecturerId: widget.userModel.id.toString(),
        schedulerId: widget.scheduleModel.id.toString());
    setState(() {});
  }

  List<dynamic> _getEvenForDay(DateTime date) {
    return getCourseByMonday(date) ?? [];
  }

  getFocusDayBySemester() {
    if (widget.semesterChose.dateStartFormat != null) {
      DateTime day = DateTime.parse(widget.semesterChose.dateStart.toString());
      _focusedDay = DateTime.parse(widget.semesterChose.dateStart.toString());
      listTime = getCurrentWeek(_focusedDay);
      return _focusedDay;
    } else {
      _focusedDay = now;
      listTime = getCurrentWeek(_focusedDay);
      return _focusedDay;
    }
  }

  @override
  Widget build(BuildContext context) {
    // if (widget.semesterChose.dateStartFormat != null) {
    //   DateTime day = DateTime.parse(widget.semesterChose.dateStart.toString());
    //   _focusedDay = DateTime.parse(widget.semesterChose.dateStart.toString());
    //   _selectedDay = DateTime.parse(widget.semesterChose.dateStart.toString());
    //   print('DateStart: ${widget.semesterChose.dateStart.toString()}');
    // }
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        SizedBox(
          height: 5,
        ),
        Padding(
          padding: const EdgeInsets.only(left: 25),
          child: Text(
            "Current Week: ${DateFormat('dd/MM').format(listTime[0])} - ${DateFormat('dd/MM').format(listTime[1])}",
            style: TextStyle(fontSize: 18, color: Colors.orange.shade600),
          ),
        ),
        TableCalendar(
          eventLoader: _getEvenForDay,
          onPageChanged: (focusedDay) {
            //No setState() here
            _focusedDay = focusedDay;
            listTime = getCurrentWeek(focusedDay);
          },
          //rowHeight: 80,
          daysOfWeekHeight: 20,
          //headerVisible: false,
          onDaySelected: (selectedDay, focusedDay) {
            setState(() {
              _selectedDay = selectedDay;
              _focusedDay = focusedDay;
              listTime = getCurrentWeek(_focusedDay);
            });
          },
          selectedDayPredicate: (day) => isSameDay(day, _selectedDay),
          focusedDay: widget.semesterChose.dateStatus == 'On Going'
              ? _selectedDay
              : getFocusDayBySemester(),
          firstDay: firstDay,
          lastDay: lastDay,
          calendarFormat: format,
          startingDayOfWeek: StartingDayOfWeek.monday,
          availableCalendarFormats: const {
            CalendarFormat.month: 'month',
            CalendarFormat.week: 'week',
            CalendarFormat.twoWeeks: '2 weeks'
          },
          headerStyle: HeaderStyle(
            leftChevronIcon: Icon(
              Icons.chevron_left,
              size: 24,
              color: Colors.black54,
            ),
            rightChevronIcon: Icon(
              Icons.chevron_right,
              size: 24,
              color: Colors.black54,
            ),
            headerPadding: EdgeInsets.zero,
            formatButtonVisible: true,
            formatButtonShowsNext: false,
            formatButtonDecoration: BoxDecoration(
              color: Colors.black54,
              shape: BoxShape.rectangle,
              borderRadius: BorderRadius.circular(15),
            ),
            formatButtonTextStyle: TextStyle(
              color: Colors.white,
              fontSize: 12,
            ),
            titleTextStyle: TextStyle(color: Colors.black),
            titleCentered: true,
          ),
          calendarStyle: CalendarStyle(
            //outsideTextStyle: TextStyle(fontSize: 16),// not same month
            selectedDecoration: BoxDecoration(
              color: Colors.orange.shade600,
              shape: BoxShape.rectangle,
            ),
            selectedTextStyle: TextStyle(
                color: Color.fromRGBO(238, 230, 226, 1), fontSize: 17),
            todayDecoration: BoxDecoration(
              color: Colors.grey.shade300,
              shape: BoxShape.rectangle,
            ),
            todayTextStyle: TextStyle(color: Colors.black, fontSize: 17),
            defaultDecoration: BoxDecoration(
              color: Colors.transparent,
              shape: BoxShape.rectangle,
            ),
            defaultTextStyle: TextStyle(color: Colors.black, fontSize: 17),
            weekendDecoration: BoxDecoration(
              color: Colors.transparent,
              shape: BoxShape.rectangle,
            ),
            weekendTextStyle: TextStyle(color: Colors.black, fontSize: 17),
          ),
          calendarBuilders: CalendarBuilders(
            dowBuilder: (context, day) {
              String text;
              if (day.weekday == DateTime.sunday) {
                text = 'Sun';
              } else if (day.weekday == DateTime.monday) {
                text = 'Mon';
              } else if (day.weekday == DateTime.tuesday) {
                text = 'Tue';
              } else if (day.weekday == DateTime.wednesday) {
                text = 'Wed';
              } else if (day.weekday == DateTime.thursday) {
                text = 'Thu';
              } else if (day.weekday == DateTime.friday) {
                text = 'Fri';
              } else if (day.weekday == DateTime.saturday) {
                text = 'Sat';
              } else {
                text = 'err';
              }
              return Center(
                child: Text(
                  text,
                  style: TextStyle(color: Colors.black38, fontSize: 17),
                ),
              );
            },
            markerBuilder: (context, day, events) {
              if (events.isNotEmpty) {
                return Stack(
                  alignment: Alignment.center,
                  children: [
                    Positioned(
                      bottom: 2.0,
                      child: Container(
                        height: 10,
                        width: 10,
                        decoration: BoxDecoration(
                          color: Colors.green,
                          shape: BoxShape.rectangle,
                          borderRadius: BorderRadius.circular(15),
                        ),
                      ),
                    ),
                  ],
                );
              }
            },
          ),
        ),
        Container(
          margin: EdgeInsets.only(top: 10, left: 20, right: 20),
          child: Row(
            children: [
              Text(
                'Time',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  fontFamily: GoogleFonts.lato().fontFamily,
                  color: Colors.grey,
                ),
              ),
              SizedBox(width: 30),
              Text(
                'Course',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  fontFamily: GoogleFonts.lato().fontFamily,
                  color: Colors.grey,
                ),
              ),
            ],
          ),
        ),
        ListCourse(
          schedulerList: getCourseByMonday(_selectedDay),
        )
      ],
    );
  }

  getCourseByMonday(DateTime date) {
    //print('day: $date');

    List<SchedulerModel> scheduler = [];
    for (var i = 0; i < widget.listSlotType.length; i++) {
      for (var j = 0; j < listCourseAssign.length; j++) {
        if (listCourseAssign[j].slotTypeId == widget.listSlotType[i].id) {
          if (widget.listSlotType[i].convertDateOfWeek
                      .toString()
                      .split(' ')[0] ==
                  DateFormat('EEEE').format(date) ||
              widget.listSlotType[i].convertDateOfWeek
                      .toString()
                      .split(' ')[2] ==
                  DateFormat('EEEE').format(date)) {
            SchedulerModel schedulerModel = SchedulerModel(
              course: listCourseAssign[j].courseId.toString().split('_')[0],
              clas: listCourseAssign[j].courseId.toString().split('_')[1],
              slotTypeId: widget.listSlotType[i].id.toString(),
              timeStart: widget.listSlotType[i].timeStart.toString(),
              timeEnd: widget.listSlotType[i].timeEnd.toString(),
              slotNumber:
                  int.parse(widget.listSlotType[i].slotNumber.toString()),
              duration: widget.listSlotType[i].duration.toString(),
            );
            scheduler.add(schedulerModel);
            //course.add(listCourseAssign[j]);
          }
        }
      }
    }
    //print('List courseAssign ${listCourseAssign.length}');
    //print('List slotType ${widget.listSlotType.length}');
    //print('Course ${course.length}');
    //print('Scheduler: ${scheduler.length}');
    // for (var se in scheduler) {
    //   print("${se.course} - ${se.clas}, Slot: ${se.slotNumber}");
    // }
    return scheduler;
  }

  getCurrentWeek(DateTime now) {
    List<DateTime> listTime = [];
    int today = now.weekday;
    //print('today: $today');
    var dayNr = (today + 6) % 7;
    //print('dayNr: $dayNr');
    var thisMonday = now.subtract(Duration(days: (dayNr)));
    //print('Monday: $thisMonday');
    listTime.add(thisMonday);
    var thisSunday = thisMonday.add(Duration(days: 6));
    //print('Sunday: $thisSunday');
    listTime.add(thisSunday);
    return listTime;
  }

  getWeek(DateTime now) {
    var week = 1;
    if (widget.semesterChose.dateStartFormat != null) {
      var sum = daysBetween(
          now, DateTime.parse(widget.semesterChose.dateStart.toString()));
      if (sum >= 7) {
        week = sum / 7;
      }
    }
    return week;
  }

  daysBetween(DateTime from, DateTime to) {
    from = DateTime(from.year, from.month, from.day);
    to = DateTime(to.year, to.month, to.day);
    return (to.difference(from).inHours / 24).round();
  }
}
