import 'package:flutter/material.dart';

import '../../model/department.dart';
import '../../model/user.dart';
import '../../services/api_handler.dart';
import 'card_depart.dart';

class ListDepart extends StatefulWidget {
  ListDepart({
    Key? key,
    required this.listdepartment,
    required this.departUser,
  }) : super(key: key);
  List<DepartmentModel> listdepartment;
  DepartmentModel departUser;
  @override
  State<ListDepart> createState() => _ListDepartState();
}

class _ListDepartState extends State<ListDepart> {
  @override
  Widget build(BuildContext context) {
    return newMethod(context);
  }

  SizedBox newMethod(BuildContext context) {
    return SizedBox(
      height: MediaQuery.of(context).size.height * 0.832,
      child: ListView.builder(
        itemCount: widget.listdepartment.length,
        itemBuilder: (context, index) {
          return FutureBuilder<UserModel>(
            future: APIHandler.getManagerByDepartId(
                departId: widget.listdepartment[index].id.toString()),
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
                return CardDepartment(
                  department: widget.listdepartment[index],
                  managerName: '',
                  departUser: widget.departUser,
                );
              }
              return snapshot.data!.name == null
                  ? CardDepartment(
                      department: widget.listdepartment[index],
                      managerName: '',
                      departUser: widget.departUser,
                    )
                  : CardDepartment(
                      department: widget.listdepartment[index],
                      managerName: snapshot.data!.name.toString(),
                      departUser: widget.departUser,
                    );
            }),
          );
        },
      ),
    );
  }
}
