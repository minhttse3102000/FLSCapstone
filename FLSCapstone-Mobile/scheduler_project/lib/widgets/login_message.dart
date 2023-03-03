// ignore_for_file: file_names, use_build_context_synchronously

import 'package:flutter/material.dart';
import '../google/google_sigin_api.dart';
import '../screen/login_screen.dart';

loginMessage(BuildContext context, String contentMessage) {
  // ignore: deprecated_member_use
  Widget yesButton = FlatButton(
    child: Text("OK", style: TextStyle(color: Color(0xffb744b8))),
    onPressed: () async {
      Navigator.of(context, rootNavigator: true).pop();
      await GoogleSignInApi.logout();
      Navigator.of(context).pushAndRemoveUntil(
          MaterialPageRoute(builder: (context) => LoginScreen()),
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
