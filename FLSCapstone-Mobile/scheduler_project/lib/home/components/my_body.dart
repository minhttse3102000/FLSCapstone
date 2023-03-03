// ignore_for_file: must_be_immutable, prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import 'package:scheduler_project/home/components/dashboard_card.dart';
import 'package:scheduler_project/home/components/menu_icon.dart';
import 'package:scheduler_project/profile/profile_page.dart';
import 'package:scheduler_project/scheduler/scheduler_page.dart';
import 'package:scheduler_project/semester/semester_page.dart';

import '../../department/department_page.dart';
import '../../model/user.dart';
import '../../subject/subject_screen.dart';

class MyBody extends StatelessWidget {
  List<Choice> choices = const <Choice>[
    Choice(title: 'Profile', icon: Icons.person),
    Choice(title: 'Timetable', icon: Icons.date_range),
    Choice(title: 'Notification', icon: Icons.notifications),
    Choice(title: 'Semester', icon: Icons.phone),
    Choice(title: 'My Department', icon: Icons.camera_alt),
  ];
  final UserModel userModel;
  MyBody({required this.userModel});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ListView(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(30, 10, 30, 10),
            child: Container(
              alignment: Alignment(1, 0),
              child: Padding(
                padding: const EdgeInsets.only(top: 20, right: 10, left: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) =>
                                  SchedulerPage(userModel: userModel)),
                        );
                      },
                      child: DashboardCard(
                        name: 'Timetable',
                        imgpath: 'images/calendar.png',
                      ),
                    ),
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                SemesterPage(userModel: userModel),
                          ),
                        );
                      },
                      child: DashboardCard(
                        name: 'Semester',
                        imgpath: 'images/semester.png',
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(30, 10, 30, 10),
            child: Container(
              alignment: Alignment(1, 0),
              child: Padding(
                padding: const EdgeInsets.only(top: 20, right: 10, left: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                DepartmentPage(userModel: userModel),
                          ),
                        );
                      },
                      child: DashboardCard(
                        name: 'Department',
                        imgpath: 'images/department.png',
                      ),
                    ),
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                SubjectScreen(userModel: userModel),
                          ),
                        );
                      },
                      child: DashboardCard(
                        name: 'Subject',
                        imgpath: 'images/subject.png',
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(30, 10, 30, 10),
            child: Container(
              alignment: Alignment(1, 0),
              child: Padding(
                padding: const EdgeInsets.only(top: 20, right: 10, left: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    GestureDetector(
                      onTap: () {},
                      child: DashboardCard(
                        name: 'Notification',
                        imgpath: 'images/notification.png',
                      ),
                    ),
                    GestureDetector(
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) =>
                                  ProfilePage(userModel: userModel)),
                        );
                      },
                      child: DashboardCard(
                        name: 'Profile',
                        imgpath: 'images/profile-item.png',
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
