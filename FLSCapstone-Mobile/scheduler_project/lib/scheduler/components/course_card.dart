// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';

class CourseCard extends StatelessWidget {
  CourseCard({
    Key? key,
    required this.course,
    required this.slot,
    required this.clas,
    required this.duration,
  }) : super(key: key);
  final String course;
  final int slot;
  final String clas;
  final String duration;
  @override
  Widget build(BuildContext context) {
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
              color: Colors.green,
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  course,
                  style: TextStyle(
                      fontSize: 18,
                      color: Colors.white,
                      fontWeight: FontWeight.bold),
                ),
                Text(
                  clas,
                  style: TextStyle(fontSize: 16, color: Colors.white),
                ),
                Text(
                  duration,
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
