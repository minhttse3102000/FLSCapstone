import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

import '../../model/course_asign.dart';
import '../../model/scheduler.dart';
import '../../model/scheduler_model.dart';
import '../../model/slot_type.dart';
import '../../model/user.dart';
import '../../services/api_handler.dart';
import 'list_course.dart';

class ListWidget extends StatefulWidget {
  const ListWidget({
    Key? key,
    required this.scheduleModel,
    required this.date,
    required this.userModel,
    required this.listSlotType,
  }) : super(key: key);
  final ScheduleModel scheduleModel;
  final DateTime date;
  final UserModel userModel;
  final List<SlotTypeModel> listSlotType;
  @override
  State<ListWidget> createState() => _ListWidgetState();
}

class _ListWidgetState extends State<ListWidget> {
  List<CourseAssignModel> listCourseAssign = [];
  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getSchedulerBySemes();
  }

  Future<void> getSchedulerBySemes() async {
    listCourseAssign = await APIHandler.getAllCourseAssignByLeSche(
        lecturerId: widget.userModel.id.toString(),
        schedulerId: widget.scheduleModel.id.toString());
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return ListCourse(
        schedulerList:
            getCourseByMonday(DateFormat('EEEE').format(widget.date)));
  }

  getCourseByMonday(String day) {
    print('day: $day');

    List<SchedulerModel> scheduler = [];
    for (var i = 0; i < widget.listSlotType.length; i++) {
      for (var j = 0; j < listCourseAssign.length; j++) {
        if (listCourseAssign[j].slotTypeId == widget.listSlotType[i].id) {
          if (widget.listSlotType[i].convertDateOfWeek
                      .toString()
                      .split(' ')[0] ==
                  day ||
              widget.listSlotType[i].convertDateOfWeek
                      .toString()
                      .split(' ')[2] ==
                  day) {
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
    print('List courseAssign ${listCourseAssign.length}');
    print('List slotType ${widget.listSlotType.length}');
    //print('Course ${course.length}');
    print('Scheduler: ${scheduler.length}');
    // for (var se in scheduler) {
    //   print("${se.course} - ${se.clas}, Slot: ${se.slotNumber}");
    // }
    return scheduler;
  }
}
