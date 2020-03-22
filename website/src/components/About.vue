<template>
    <div id="about">
        <h1>About Our Project</h1>
        <p align="left">
            <br />
            SeeShells is essentially an information extract software. The objective is to
            create a standalone open source executable that can run both online and offline.
            It extracts and parses through Windows Registry information. This data This data 
            is then converted into two forms.The first is a csv file that contains all the raw data we obtain. 
            The second is a human readable timeline that can be downloaded and used as evidence in a courtroom.
            The timeline provides an interactive easier to read visualization of the data extracted from the 
            windows registries, which is otherwise difficult and time consuming to comb through and understand. 
            The parsing and extraction of information has a slightly different
            process for each of the windows versions including Windows XP, Windows Vista
            Windows 7,8,8.1 and 10. In order to create a robust application we have set up a
            server to store database information on parsing different registry versions.
        </p>
        <p align="left">
            The software expediates the process of extracting, parsing, and presenting the 
            registry information in a way that is condensed and easily understandable. 
            We hope others will benefit from our interactive timeline generated from the ShellBag 
            information and we hope to make a great impact on the digital forensics community.
            <br />
            <br />
        </p>
        <p/>
        <h2 align="left">Objectives of SeeShells</h2>
        <p></p>
        <ul>
            <li align="left">
                Create a standalone WPF application that can extract and parse ShellBags
                registry information.
            </li>
            <li align="left">
                Support multiple Windows Operating Systems as well as reading both live
                system registries and offline hives.
            </li>
            <li align="left">
                Draw a human-readable, graphical timeline for users to look at as well as a
                document that can be inserted into a technical report.
            </li>
            <li align="left">
                Set up and utilize a server to store database information on parsing different
                registry versions to create a robust application.
            </li>
            
        </ul>
    </div>
</template>

<script>
export default {
  name: 'about'
}
</script>
