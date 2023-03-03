// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/model/semester.dart';

import '../../model/user.dart';
import '../semester_detail_page.dart';

class SemesterCard extends StatelessWidget {
  final UserModel userModel;
  const SemesterCard({
    Key? key,
    required this.userModel,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    SemesterModel semesterProvider = Provider.of<SemesterModel>(context);
    return GestureDetector(
      onTap: () {
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => SemesterDetail(
                semesterModel: semesterProvider, userModel: userModel),
          ),
        );
      },
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 15),
        child: Container(
          padding: EdgeInsets.all(10),
          constraints: BoxConstraints(
            maxHeight: double.infinity,
          ),
          height: size.height * 0.3,
          width: size.height * 0.22,
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(16),
            boxShadow: [
              BoxShadow(
                color: Colors.grey,
                blurRadius: 12,
                offset: Offset(0, 6),
              ),
            ],
          ),
          child: Stack(
            alignment: Alignment.center,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Image.asset(
                    "assets/images/${semesterProvider.term.toString().split(' ')[0]}.png",
                    width: 80,
                  ),
                  SizedBox(
                    height: 5,
                  ),
                  Text(
                    semesterProvider.term.toString(),
                    style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
                  ),
                  SizedBox(
                    height: 5,
                  ),
                  Text(
                    "${DateFormat("dd/MM").format(DateTime.parse(semesterProvider.dateStart.toString()))} - ${DateFormat("dd/MM").format(DateTime.parse(semesterProvider.dateEnd.toString()))}",
                    style: TextStyle(fontSize: 20),
                  ),
                ],
              ),
              Positioned(
                bottom: 0,
                left: 0,
                right: 0,
                child: Container(
                  padding: EdgeInsets.all(4),
                  color: customColor(semesterProvider.dateStatus.toString()),
                  child: semesterProvider.dateStatus == 'On Going'
                      ? Text(
                          semesterProvider.dateStatus.toString(),
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                              fontSize: 18),
                        )
                      : Text(
                          semesterProvider.dateStatus.toString(),
                          textAlign: TextAlign.center,
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 18,
                          ),
                        ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  customColor(String status) {
    var color;
    if (status == 'On Going') {
      color = Colors.orange[400];
    } else if (status == 'Close') {
      color = Colors.grey;
    } else {
      color = Colors.deepOrange[400];
    }
    return color;
  }
}
