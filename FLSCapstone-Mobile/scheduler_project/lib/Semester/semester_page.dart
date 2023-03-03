// ignore_for_file: prefer_const_literals_to_create_immutables, prefer_const_constructors_in_immutables, unused_local_variable, unused_element

import 'package:flutter/material.dart';
import 'package:scheduler_project/semester/components/semester_list.dart';

import '../model/semester.dart';
import '../model/user.dart';
import '../services/api_handler.dart';

class SemesterPage extends StatelessWidget {
  final backColor = Colors.lightGreen;
  final UserModel userModel;
  SemesterPage({required this.userModel});

  @override
  Widget build(BuildContext context) {
    final double width = MediaQuery.of(context).size.width;
    final double height = MediaQuery.of(context).size.height;

    return Scaffold(
      appBar: AppBar(
        title: Text('Semester'),
        centerTitle: true,
        backgroundColor: Colors.green[700],
        leading: IconButton(
          onPressed: () {
            Navigator.pop(context);
          },
          icon: Icon(
            Icons.arrow_back_ios,
            color: Colors.white,
          ),
        ),
      ),
      body: Column(
        children: [
          FutureBuilder<List<SemesterModel>>(
            future: APIHandler.getAllSemesterDec(),
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
                  child: Text("No products has been added yet"),
                );
              }
              return SemesterList(
                semesterList: snapshot.data!,
                userModel: userModel,
              );
            }),
          ),
        ],
      ),
    );
  }
}
