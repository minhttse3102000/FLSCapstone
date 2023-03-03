import 'package:flutter/material.dart';
import 'package:scheduler_project/widgets/main_drawer.dart';

class TestDrawer extends StatelessWidget {
  const TestDrawer({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Drawer Test'),
      ),
      drawer: Drawer(
        elevation: 2,
        child: MainDrawer(),
      ),
    );
  }
}
