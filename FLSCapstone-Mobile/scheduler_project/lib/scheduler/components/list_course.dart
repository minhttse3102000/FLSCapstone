// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import 'package:scheduler_project/model/scheduler_model.dart';

import 'course_card.dart';

class ListCourse extends StatelessWidget {
  const ListCourse({
    Key? key,
    required this.schedulerList,
  }) : super(key: key);
  final List<SchedulerModel> schedulerList;
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: MediaQuery.of(context).size.height * 0.575, //0.575
      child: ListView.builder(
        itemCount: 5,
        itemBuilder: (context, index) {
          if (schedulerList.length < 1) {
            return Container();
          } else {
            for (var i = 0; i < schedulerList.length; i++) {
              if (index + 1 == schedulerList[i].slotNumber) {
                return CourseCard(
                  course: schedulerList[i].course,
                  slot: schedulerList[i].slotNumber,
                  clas: schedulerList[i].clas,
                  duration: schedulerList[i].duration,
                );
              } else {
                // if (index < schedulerList[i].slotNumber) {
                //   return cardScheduler(context, index + 1);
                // }
              }
            }
          }
          return Container();
        },
      ),
    );
  }

  cardScheduler(BuildContext context, int slot) {
    return Container(
      margin: EdgeInsets.only(left: 20), //right
      child: Row(
        children: [
          Text(
            'Slot $slot',
            style: TextStyle(fontSize: 18),
          ),
          SizedBox(width: 40),
          Container(
            padding: EdgeInsets.all(16),
            margin: EdgeInsets.only(top: 20),
            width: MediaQuery.of(context).size.width - 120,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(16),
              color: Colors.green[300],
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  '',
                  style: TextStyle(
                      fontSize: 18,
                      color: Colors.white,
                      fontWeight: FontWeight.bold),
                ),
                Text(
                  '',
                  style: TextStyle(fontSize: 16, color: Colors.white),
                ),
                Text(
                  '',
                  style: TextStyle(fontSize: 15, color: Colors.white),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}


// for (var i = 0; i < schedulerList.length; i++) {
//                 if (index + 1 == schedulerList[i].slotNumber) {
//                   return CourseCard(
//                     course: schedulerList[i].course,
//                     slot: schedulerList[i].slotNumber,
//                     clas: schedulerList[i].clas,
//                     duration: schedulerList[i].duration,
//                   );
//                 }
//               }