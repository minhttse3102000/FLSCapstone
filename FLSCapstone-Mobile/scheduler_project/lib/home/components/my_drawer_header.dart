import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/model/user.dart';

class MyHeaderDrawer extends StatelessWidget {
  const MyHeaderDrawer({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    UserModel userProvider = Provider.of<UserModel>(context);
    return Container(
      color: Colors.green[700],
      width: double.infinity,
      height: 200,
      padding: EdgeInsets.only(top: 20),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            margin: EdgeInsets.only(bottom: 10),
            height: 70,
            width: 70,
            child: CircleAvatar(
              backgroundImage: AssetImage("assets/images/gaixinh.jpg"),
            ),
          ),
          Text(
            userProvider.name.toString(),
            style: TextStyle(
              color: Colors.white,
              fontSize: 20,
            ),
          ),
          Text(
            userProvider.email.toString(),
            style: TextStyle(
              color: Colors.grey[200],
              fontSize: 14,
            ),
          )
        ],
      ),
    );
  }
}
