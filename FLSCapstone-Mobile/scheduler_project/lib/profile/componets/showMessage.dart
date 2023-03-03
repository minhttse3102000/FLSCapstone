// ignore_for_file: file_names

import 'package:flutter/material.dart';
import 'package:scheduler_project/model/user.dart';
import 'package:scheduler_project/profile/profile_page.dart';

showMessage(BuildContext context, String contentMessage, UserModel userModel) {
  // ignore: deprecated_member_use
  Widget yesButton = FlatButton(
    child: Text("OK", style: TextStyle(color: Color(0xffb744b8))),
    onPressed: () {
      Navigator.of(context, rootNavigator: true).pop();
      Navigator.of(context).pushAndRemoveUntil(
          MaterialPageRoute(
              builder: (context) => ProfilePage(
                    userModel: userModel,
                  )),
          (Route<dynamic> route) => false);
    },
  );

  // set up the AlertDialog
  AlertDialog alert = AlertDialog(
    title: Text("Message"),
    content: Text(contentMessage),
    actions: [
      yesButton,
    ],
  );
  // show the dialog
  showDialog(
    context: context,
    builder: (BuildContext context) {
      return alert;
    },
  );
}
