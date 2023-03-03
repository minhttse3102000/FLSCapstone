// ignore_for_file: prefer_const_literals_to_create_immutables, avoid_unnecessary_containers, unused_local_variable

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/model/subject.dart';

class CardSubject extends StatelessWidget {
  const CardSubject({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    SubjectModel subjectProvider = Provider.of<SubjectModel>(context);
    return Padding(
      padding: const EdgeInsets.only(left: 10, top: 10, right: 10),
      child: Container(
        decoration: BoxDecoration(
          boxShadow: [
            BoxShadow(
              color: Colors.grey,
              blurRadius: 12,
              offset: Offset(0, 6),
            ),
          ],
          //borderRadius: BorderRadius.circular(30),
        ),
        height: 100,
        child: Card(
          child: Container(
            child: Row(
              children: [
                Container(
                  margin: EdgeInsets.only(left: 10),
                  width: 70,
                  height: 70,
                  decoration: BoxDecoration(
                    border: Border.all(width: 1, color: Colors.white),
                    shape: BoxShape.circle,
                    // You can use like this way or like the below line
                    //borderRadius: new BorderRadius.circular(30.0),
                    color: Colors.green[500],
                  ),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        subjectProvider.id.toString(),
                        style: TextStyle(color: Colors.white),
                      ),
                    ],
                  ),
                ),
                SizedBox(width: 4),
                Flexible(
                  child: Container(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          subjectProvider.subjectName.toString(),
                          style: TextStyle(fontSize: 18.5),
                          overflow: TextOverflow.ellipsis,
                          maxLines: 3,
                        ),
                      ],
                    ),
                  ),
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
