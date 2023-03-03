// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import 'package:flutter/src/foundation/key.dart';
import 'package:flutter/src/widgets/framework.dart';

import '../../model/department.dart';

class CardDepartment extends StatelessWidget {
  CardDepartment({
    Key? key,
    required this.department,
    required this.managerName,
    required this.departUser,
  }) : super(key: key);
  final DepartmentModel department;
  final String managerName;
  DepartmentModel departUser;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 15),
      child: Stack(
        children: [
          Container(
            height: MediaQuery.of(context).size.height * 0.15,
            width: double.infinity,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(20),
              gradient: LinearGradient(
                colors: [Colors.white, Colors.white],
                begin: Alignment.topLeft,
                end: Alignment.topRight,
              ),
              boxShadow: [
                BoxShadow(
                  color: Colors.grey,
                  blurRadius: 12,
                  offset: Offset(0, 6),
                ),
              ],
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.center,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Row(
                  children: [
                    Container(
                      margin: EdgeInsets.only(left: 10, top: 10),
                      width: 60,
                      height: 60,
                      decoration: BoxDecoration(
                        border: Border.all(width: 1, color: Colors.green),
                        shape: BoxShape.circle,
                        // You can use like this way or like the below line
                        //borderRadius: new BorderRadius.circular(30.0),
                        color: Colors.green,
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          departUser.id == department.id
                              ? Text(
                                  department.id.toString(),
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontWeight: FontWeight.bold,
                                  ),
                                )
                              : Text(
                                  department.id.toString(),
                                  style: TextStyle(
                                    color: Colors.white,
                                  ),
                                ),
                        ],
                      ),
                    ),
                    SizedBox(
                      width: 10,
                    ),
                    Expanded(
                      child: Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            departUser.id == department.id
                                ? Text(
                                    department.departmentName.toString(),
                                    style: TextStyle(
                                        color: Colors.black,
                                        fontSize: 20,
                                        fontWeight: FontWeight.bold),
                                  )
                                : Text(
                                    department.departmentName.toString(),
                                    style: TextStyle(
                                      color: Colors.black87,
                                      fontSize: 20,
                                    ),
                                  ),
                            SizedBox(
                              height: 5,
                            ),
                            departUser.id == department.id
                                ? Text(
                                    'Manager $managerName',
                                    style: TextStyle(
                                        color: Colors.black,
                                        fontSize: 18,
                                        fontWeight: FontWeight.bold),
                                  )
                                : Text(
                                    'Manager $managerName',
                                    style: TextStyle(
                                        color: Colors.black87, fontSize: 18),
                                  ),
                            SizedBox(
                              height: 5,
                            ),
                            departUser.id == department.id
                                ? Text(
                                    'Group ${department.departmentGroupId.toString()}',
                                    style: TextStyle(
                                        color: Colors.black,
                                        fontSize: 16,
                                        fontWeight: FontWeight.bold),
                                  )
                                : Text(
                                    'Group ${department.departmentGroupId.toString()}',
                                    style: TextStyle(
                                        color: Colors.black87, fontSize: 16),
                                  ),
                          ],
                        ),
                      ),
                    ),
                  ],
                )
              ],
            ),
          ),
        ],
      ),
    );
  }
}
