<template>
    <div id="content-wrapper">

        <div class="container-fluid">

            <!-- Page Content -->
            <table id="users" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Approve</th>
                        <th>Reject</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                </tbody>

            </table>

        </div>
    </div>
</template>

<!-- DataTables -->
<script src="vendor/datatables/jquery.dataTables.js"></script>
<script src="vendor/datatables/dataTables.bootstrap4.js"></script>
<script>
    export default {
        name: "NewUsers",
        data() {
            return {
                baseurl: 'http://localhost:3000/' //https://seeshells.herokuapp.com/
            }
        },
        methods: {
            populateTable() {
                var url = this.baseurl + 'getNewUsers';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        var table = document.getElementById("users");
                        var arr = Array.from(result.json);
                        arr.forEach($.proxy(function (item, index) {
                            this.addRowOnTable(table, item, index)
                        }, this));

                        //$(document).ready(function () {
                        //    $('#users').DataTable();
                        //});
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },

            addRowOnTable(table, item, index) {
                if (item != null) {

                    $(table).find('tbody').append("<tr><td>" + item.username +
                        "</td><td><a class='btn btn-sm btn-primary' style='color:white' v-on:click='approveUser('" + item.id +
                        "')'>&nbsp;&nbsp;&nbsp;O&nbsp;&nbsp;&nbsp;</a> </td><td> <a class='btn btn-sm btn-primary' style='color:white' v-on:click='rejectUser('" +
                        item.id + "')'>&nbsp;&nbsp;&nbsp;X&nbsp;&nbsp;&nbsp;</a> </td></tr>");

                    ////var approvebtn = document.getElementById("approvebutton" + item.id);
                    ////var rejectbtn = document.getElementById("rejectbutton" + item.id);
                    ////approvebtn.onclick = function () { this.approveUser(item.id) };
                    ////rejectbtn.onclick = function () { this.rejectUser(item.id) };

                }
            },

            approveUser(userID) {
                var url = this.baseurl + 'approveUser';

                var jsonPayload = '{"userID":' + userID + '"}';

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(jsonPayload);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.populateTable();
                    }
                    else if (result.message == "You must log in to perform this action.") {
                        this.$session.destroy();
                        this.$router.push('/SeeShells/login');
                        location.reload();
                    }
                }
                catch (err) {
                    console.log(err);
                }
            },

            rejectUser(userID) {
                alert('test!');
                var url = this.baseurl + 'rejectUser';

                var jsonPayload = '{"userID":' + userID + '"}';

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(jsonPayload);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.populateTable();
                    }
                    else if (result.message == "You must log in to perform this action.") {
                        this.$session.destroy();
                        this.$router.push('/SeeShells/login');
                        location.reload();
                    }
                }
                catch (err) {
                    console.log(err);
                }
            }
        },
        mounted() {
            this.$nextTick().then(this.populateTable);
        }
    }
</script>