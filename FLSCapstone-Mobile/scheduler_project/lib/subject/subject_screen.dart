// ignore_for_file: prefer_const_literals_to_create_immutables, avoid_unnecessary_containers

import 'package:flutter/material.dart';
import 'package:scheduler_project/model/subject.dart';
import 'package:scheduler_project/services/api_handler.dart';
import 'package:scheduler_project/subject/components/list_card.dart';

import '../model/department.dart';
import '../model/user.dart';

class SubjectScreen extends StatefulWidget {
  final UserModel userModel;
  const SubjectScreen({
    Key? key,
    required this.userModel,
  }) : super(key: key);

  @override
  State<SubjectScreen> createState() => _SubjectScreenState();
}

class _SubjectScreenState extends State<SubjectScreen> {
  DepartmentModel? _selectedValue;
  List<DepartmentModel> listdepartment = [];
  DepartmentModel depart = DepartmentModel();
  List<SubjectModel> listsubjectDp = [];
  DepartmentModel departSubject = DepartmentModel();

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getListDepartment();
    getListSubjectByDepart();
  }

  Future<void> getListDepartment() async {
    depart = await APIHandler.getDepartmentById(
        departId: widget.userModel.departmentId.toString());
    listdepartment = await APIHandler.getAllDepartmentByDepartGroupId(
        departmentGroupId: depart.departmentGroupId.toString());
    setState(() {});
  }

  Future<void> getListSubjectByDepart() async {
    listsubjectDp = await APIHandler.getAllSubjectByDepartId(
        departmentId: depart.id.toString());
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    if (_selectedValue == null) {
      departSubject = depart;
    } else {
      departSubject = _selectedValue!;
    }
    return Scaffold(
      appBar: AppBar(
        title: Text('Subject'),
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
        padding: const EdgeInsets.only(top: 20, left: 20, right: 20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: <Widget>[
                Flexible(
                  child: Row(
                    children: <Widget>[
                      Text(
                        "Department:",
                        style: TextStyle(fontSize: 23),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.only(left: 10),
                          decoration: BoxDecoration(
                            border: Border.all(width: 1, color: Colors.grey),
                            borderRadius: BorderRadius.circular(10),
                          ),
                          margin: EdgeInsets.only(left: 10),
                          child: DropdownButton<DepartmentModel>(
                            underline: Container(),
                            isExpanded: true,
                            hint: Text(
                              depart.departmentName.toString(),
                              style: TextStyle(color: Colors.black),
                            ),
                            value: _selectedValue,
                            onChanged: (newValue) {
                              setState(() {
                                _selectedValue = newValue;
                              });
                            },
                            items: listdepartment.map((DepartmentModel value) {
                              return DropdownMenuItem<DepartmentModel>(
                                value: value,
                                child: Text(value.departmentName.toString()),
                              );
                            }).toList(),
                          ),
                        ),
                      )
                    ],
                  ),
                )
              ],
            ),
            SizedBox(
              height: 10,
            ),
            //Text('Manager: ', style: TextStyle(fontSize: 21)),
            FutureBuilder<UserModel>(
              future: APIHandler.getManagerByDepartId(
                  departId: departSubject.id.toString()),
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
                    child: Text("Manager:", style: TextStyle(fontSize: 21)),
                  );
                }
                return snapshot.data!.name == null
                    ? Text('Manager:', style: TextStyle(fontSize: 21))
                    : Text('Manager: ${snapshot.data!.name}',
                        style: TextStyle(fontSize: 21));
              }),
            ),
            SizedBox(
              height: 10,
            ),
            Text(
              depart.id == departSubject.id
                  ? 'List subjects in my department'
                  : 'List subjects in not my department',
              style: TextStyle(fontSize: 20),
            ),
            SizedBox(
              height: 10,
            ),
            FutureBuilder<List<SubjectModel>>(
              future: APIHandler.getAllSubjectByDepartId(
                  departmentId: departSubject.id.toString()),
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
                    child: Text("No subject has been added yet"),
                  );
                }
                return MyCard(
                  subjectList: snapshot.data!,
                );
              }),
            ),
          ],
        ),
      ),
    );
  }
}
