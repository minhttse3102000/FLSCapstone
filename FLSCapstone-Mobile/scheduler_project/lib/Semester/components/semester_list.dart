// ignore_for_file: prefer_const_literals_to_create_immutables, prefer_const_constructors_in_immutables

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/model/semester.dart';

import '../../model/user.dart';
import 'semester_card.dart';

class SemesterList extends StatelessWidget {
  final List<SemesterModel> semesterList;
  final UserModel userModel;
  SemesterList({
    required this.semesterList,
    required this.userModel,
  });
  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.only(top: 10, left: 20),
          child: Text(
            'List all semester',
            style: TextStyle(fontSize: 22),
          ),
        ),
        SizedBox(
          height: MediaQuery.of(context).size.height * 0.8,
          child: GridView.builder(
            physics: NeverScrollableScrollPhysics(),
            shrinkWrap: true,
            gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 2,
              crossAxisSpacing: 12,
              mainAxisSpacing: 10,
              mainAxisExtent: MediaQuery.of(context).size.height * 0.3,
            ),
            itemCount: semesterList.length,
            itemBuilder: (_, index) {
              return ChangeNotifierProvider.value(
                value: semesterList[index],
                child: SemesterCard(
                  userModel: userModel,
                ),
              );
            },
          ),
        ),
      ],
    );
  }
}
