<template>
    <div id="home">
        <h1>Welcome to SeeShells</h1>
        <h4>Extract Information - Create Timeline - Apply filters - Export HTML Report or Raw data to csv </h4>
        <p/>
        <button type="button">Download SeeShells.exe</button>
        <p style="text-align:center">
            <br />
            <br />
            SeeShells is an information extraction software.
            SeeShells is a standalone, open source executable that can run both online and offline.
            It extracts and parses through Windows Registry information. This data is then converted into two forms.
            The first is a csv file that contains all the raw data we obtain. The second is a human readable timeline.
            The timeline provides an interactive, easier to read visualization of the data extracted from the windows registries. This information
            is otherwise difficult and time consuming to comb through and understand. The application is a great way to gain
            insight into what someone has done on their computer over time. The program can be particularly useful for 
            digital forensics investigators as the information can be downloaded and used as evidence in a court of law. 

        </p>
    </div>
</template>

<script>
export default {
  name: 'Home'
}
</script>

<style scoped>
    div{
        font-size: x-large;
    }

</style>
