<template>
    <div id="home">
        <h1>Welcome to SeeShells</h1>
        <h6>Extract Information - Create Timeline - Filter your information - Export Report or Raw data</h6>
        <button type="button">Download SeeShells.exe</button>
        <p>
            <br />
            <br />
            SeeShells is essentially an information extraction software.
            The objective is to create a standalone open source executable that can run both online and offline.
            It extracts and parses through Windows Registry information. This data is then converted into two forms.
            The first is a csv file that contains all the raw data we obtain. The second is a human readable timeline.
            The timeline provides an interactive easier to read visualization of the data extracted from the windows registries,
            which is otherwise difficult and time consuming to comb through and understand.
        </p>
    </div>
</template>

<script>
export default {
  name: 'Home'
}
</script>
