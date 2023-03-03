import 'package:flutter/material.dart';

import '../model/request.dart';
import 'componets/request_card.dart';
import 'request_page.dart';

class ListRequest extends StatelessWidget {
  final List<RequestModel> listRequest;
  const ListRequest({
    Key? key,
    required this.listRequest,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height * 0.5,
      child: ListView.builder(
        itemCount: listRequest.length,
        itemBuilder: (context, index) {
          return SubjectCard(
            requestModel: listRequest[index],
          );
          // return RequestPage(
          //   requestModel: listRequest[index],
          // );
        },
      ),
    );
    ;
  }
}
