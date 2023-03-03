// ignore_for_file: prefer_const_literals_to_create_immutables, sized_box_for_whitespace, avoid_unnecessary_containers, prefer_const_constructors_in_immutables

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/model/subject.dart';

import 'card_subject.dart';

class MyCard extends StatelessWidget {
  MyCard({
    Key? key,
    required this.subjectList,
  }) : super(key: key);
  final List<SubjectModel> subjectList;
  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height * 0.7,
      child: ListView.builder(
        itemCount: subjectList.length,
        itemBuilder: (context, index) {
          return ChangeNotifierProvider.value(
            value: subjectList[index],
            child: CardSubject(),
          );
        },
      ),
    );
  }
}
