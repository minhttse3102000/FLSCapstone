// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import '../model/depart_detail.dart';
import '../model/department.dart';
import '../model/user.dart';
import '../services/api_handler.dart';
import 'componets/list_depart.dart';

class DepartmentPage extends StatefulWidget {
  const DepartmentPage({
    Key? key,
    required this.userModel,
  }) : super(key: key);
  final UserModel userModel;

  @override
  State<DepartmentPage> createState() => _DepartmentPageState();
}

class _DepartmentPageState extends State<DepartmentPage> {
  DepartmentModel depart = DepartmentModel();
  List<DepartmentModel> listdepartment = [];
  List<DepartDetail> listDepartDetail = [];
  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getListDepartment();
  }

  Future<void> getListDepartment() async {
    depart = await APIHandler.getDepartmentById(
        departId: widget.userModel.departmentId.toString());
    listdepartment = await APIHandler.getAllDepartmentByDepartGroupId(
        departmentGroupId: depart.departmentGroupId.toString());
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Department'),
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
      body: Padding(
        padding: const EdgeInsets.only(left: 15, right: 15, top: 10),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'List all department',
              style: TextStyle(fontSize: 21),
            ),
            SizedBox(
              height: 10,
            ),
            ListDepart(
              listdepartment: listdepartment,
              departUser: depart,
            ),
          ],
        ),
      ),
    );
  }
}
