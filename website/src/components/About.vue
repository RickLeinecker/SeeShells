<template>
    <div id="home">
        <h1>About Our Project</h1>
        <p>
            SeeShells is essentially an information extract software. The objective is to
            create a standalone open source executable that can run both online and offline.
            It will extract and parse through Windows Registry information. This data will then
            be converted into two forms. The first is a csv file that will contain all the raw data
            we obtain. The second is a human readable timeline. The timeline will provide an
            interactive easier to read visualization of the data extracted from the windows
            registries, which is otherwise difficult and time consuming to comb through and
            understand. The parsing and extraction of information has a slightly different
            process for each of the windows versions including Windows XP, Windows Vista
            Windows 7,8,8.1 and 10. In order to create a robust application we have set up a
            server to store database information on parsing different registry versions.
        </p>
        <p>
            In the long run our hope for this software is that it will expedite the process
            of extracting, parsing, and presenting the registry information in a way that is
            condensed and easily understandable. We hope others will benefit from our
            interactive timeline generated from the ShellBag information and we hope to make
            a great impact on the digital forensics community.
        </p>
    </div>
</template>

<script>
export default {
  name: 'Home'
}
</script>
