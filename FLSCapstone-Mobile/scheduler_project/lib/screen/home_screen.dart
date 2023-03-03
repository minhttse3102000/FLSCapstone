// ignore_for_file: prefer_const_constructors_in_immutables, use_build_context_synchronously

import 'package:flutter/material.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:scheduler_project/google/google_sigin_api.dart';
import 'package:scheduler_project/screen/login_screen.dart';

class HomeScreen extends StatefulWidget {
  static String routeName = 'HomeScreen';
  final GoogleSignInAccount user;

  HomeScreen({Key? key, required this.user}) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _currentIndex = 0;
  final List<Widget> pages = [
    Center(
      child: Text(
        'Home Page',
        style: TextStyle(fontSize: 30),
      ),
    ),
    Center(
      child: Text(
        'Schedule Page',
        style: TextStyle(fontSize: 30),
      ),
    ),
    Center(
      child: Text(
        'Notification Page',
        style: TextStyle(fontSize: 30),
      ),
    ),
  ];
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Demo App'),
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
      body: pages[_currentIndex],
      // ignore: prefer_const_literals_to_create_immutables
      bottomNavigationBar: BottomNavigationBar(
        type: BottomNavigationBarType.fixed, //backgroundColor: Colors.blue,
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Home',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.date_range),
            label: 'Schedule',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: 'Profile',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.notifications),
            label: 'Notification',
          ),
        ],
        currentIndex: _currentIndex,
        onTap: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
      ),
    );
  }
}
