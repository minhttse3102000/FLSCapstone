// ignore_for_file: prefer_const_constructors_in_immutables, use_build_context_synchronously

import 'package:flutter/material.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:scheduler_project/screen/login_screen.dart';

import '../google/google_sigin_api.dart';

class LoggedScreen extends StatelessWidget {
  final GoogleSignInAccount user;

  const LoggedScreen({
    Key? key,
    required this.user,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Logged Screen'),
        centerTitle: true,
        actions: [
          TextButton(
            onPressed: () async {
              await GoogleSignInApi.logout();
              Navigator.of(context).pushReplacement(MaterialPageRoute(
                builder: (context) => LoginScreen(),
              ));
            },
            child: Text(
              'Logout',
              style: TextStyle(color: Colors.white),
            ),
          )
        ],
      ),
      body: Container(
        alignment: Alignment.center,
        color: Colors.blueGrey.shade900,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          // ignore: prefer_const_literals_to_create_immutables
          children: [
            Text(
              'Profile',
              style: TextStyle(
                fontSize: 24,
              ),
            ),
            SizedBox(
              height: 30,
            ),
            CircleAvatar(
              radius: 40,
              backgroundImage: NetworkImage(user.photoUrl!),
            ),
            SizedBox(
              height: 8,
            ),
            Text(
              'Name: ${user.displayName!}',
              style: TextStyle(
                color: Colors.white,
                fontSize: 20,
              ),
            ),
            SizedBox(
              height: 8,
            ),
            Text(
              'Email: ${user.email}',
              style: TextStyle(
                color: Colors.white,
                fontSize: 20,
              ),
            ),
            SizedBox(
              height: 8,
            ),
          ],
        ),
      ),
    );
  }
}
