(function(e){function t(t){for(var s,i,o=t[0],l=t[1],c=t[2],u=0,m=[];u<o.length;u++)i=o[u],Object.prototype.hasOwnProperty.call(r,i)&&r[i]&&m.push(r[i][0]),r[i]=0;for(s in l)Object.prototype.hasOwnProperty.call(l,s)&&(e[s]=l[s]);d&&d(t);while(m.length)m.shift()();return n.push.apply(n,c||[]),a()}function a(){for(var e,t=0;t<n.length;t++){for(var a=n[t],s=!0,o=1;o<a.length;o++){var l=a[o];0!==r[l]&&(s=!1)}s&&(n.splice(t--,1),e=i(i.s=a[0]))}return e}var s={},r={app:0},n=[];function i(t){if(s[t])return s[t].exports;var a=s[t]={i:t,l:!1,exports:{}};return e[t].call(a.exports,a,a.exports,i),a.l=!0,a.exports}i.m=e,i.c=s,i.d=function(e,t,a){i.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:a})},i.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},i.t=function(e,t){if(1&t&&(e=i(e)),8&t)return e;if(4&t&&"object"===typeof e&&e&&e.__esModule)return e;var a=Object.create(null);if(i.r(a),Object.defineProperty(a,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var s in e)i.d(a,s,function(t){return e[t]}.bind(null,s));return a},i.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return i.d(t,"a",t),t},i.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},i.p="/SeeShells/";var o=window["webpackJsonp"]=window["webpackJsonp"]||[],l=o.push.bind(o);o.push=t,o=o.slice();for(var c=0;c<o.length;c++)t(o[c]);var d=l;n.push([0,"chunk-vendors"]),a()})({0:function(e,t,a){e.exports=a("56d7")},"013b":function(e,t,a){"use strict";var s=a("e9fb"),r=a.n(s);r.a},"034f":function(e,t,a){"use strict";var s=a("85ec"),r=a.n(s);r.a},"26be":function(e,t,a){},"2cc5":function(e,t,a){"use strict";var s=a("26be"),r=a.n(s);r.a},5251:function(e,t,a){},"56d7":function(e,t,a){"use strict";a.r(t);a("e260"),a("e6cf"),a("cca6"),a("a79d");var s=a("2b0e"),r=a("5f5b"),n=a("b1e0"),i=(a("f9e3"),a("2dd8"),a("8c4f")),o=a("0628"),l=a.n(o),c=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"app"}},[a("NavigationBar"),a("div",{attrs:{id:"page"}},[a("router-view")],1)],1)},d=[],u=function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"nav"}},[s("b-navbar",{attrs:{toggleable:"lg",type:"dark",variant:"dark"}},[s("img",{attrs:{alt:"Vue logo",src:a("cf05"),width:"75",height:"75"}}),s("router-link",{attrs:{to:"/SeeShells/"}},[s("b-navbar-brand",[e._v("SeeShells")])],1),s("b-navbar-toggle",{attrs:{target:"nav-collapse"}}),s("b-collapse",{attrs:{id:"nav-collapse","is-nav":""}},[s("router-link",{attrs:{to:"/SeeShells/about",tag:"menuitem"}},[s("b-navbar-item",[e._v("About")])],1),s("b-navbar-nav",[s("b-nav-item",{attrs:{href:"#"}},[e._v("Download the Program")])],1),s("router-link",{attrs:{to:"/SeeShells/team",tag:"menuitem"}},[s("b-navbar-item",[e._v("Developer Team")])],1),s("b-navbar-nav",[s("b-nav-item",{attrs:{href:"https://github.com/RickLeinecker/SeeShells"}},[e._v("GitHub Page")])],1),s("b-navbar-nav",{staticClass:"ml-auto"},[s("b-nav-item-dropdown",{attrs:{right:""},scopedSlots:e._u([{key:"button-content",fn:function(){return[s("em",[e._v("Administrative")])]},proxy:!0}])},[e.sessionExists?s("div",[s("LoggedIn")],1):s("div",[s("LoggedOut")],1)])],1)],1)],1)],1)},m=[],p=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"items"}},[a("b-dropdown-item",[e._v("Approve New Admins")]),a("b-dropdown-item",[e._v("Add New Script")]),a("b-dropdown-item",{on:{click:e.Logout}},[e._v("Logout")])],1)},f=[],h={name:"LoggedIn",methods:{Logout:function(){this.$session.destroy();var e="https://seeshells.herokuapp.com/",t=e+"logout",a=new XMLHttpRequest;a.open("GET",t,!1),a.setRequestHeader("Content-type","application/json; charset=UTF-8");try{a.send()}catch(s){alert(s.message)}location.reload()}}},v=h,g=a("2877"),b=Object(g["a"])(v,p,f,!1,null,null,null),w=b.exports,_=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"items"}},[a("b-dropdown-item",[a("router-link",{attrs:{to:"/SeeShells/register"}},[e._v("Register")])],1),a("b-dropdown-item",[a("router-link",{attrs:{to:"/SeeShells/login"}},[e._v("Login")])],1)],1)},y=[],S={name:"LoggedOut",methods:{}},x=S,j=Object(g["a"])(x,_,y,!1,null,null,null),C=j.exports,T={name:"NavigationBar",computed:{sessionExists:function(){return this.$session.exists()}},components:{LoggedIn:w,LoggedOut:C}},O=T,E=(a("2cc5"),Object(g["a"])(O,u,m,!1,null,"7e8e7d60",null)),k=E.exports,P={name:"App",components:{NavigationBar:k}},$=P,L=(a("034f"),Object(g["a"])($,c,d,!1,null,null,null)),R=L.exports,A=function(){var e=this,t=e.$createElement;e._self._c;return e._m(0)},W=[function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"home"}},[a("h1",[e._v("Welcome to SeeShells")]),a("h6",[e._v("Extract Information - Create Timeline - Filter your information - Export Report or Raw data")]),a("button",{attrs:{type:"button"}},[e._v("Download SeeShells.exe")]),a("p",[a("br"),a("br"),e._v(" SeeShells is essentially an information extraction software. The objective is to create a standalone open source executable that can run both online and offline. It will extract and parse through Windows Registry information. This data will then be converted into two forms. The first is a csv file that will contain all the raw data we obtain. The second is a human readable timeline. The timeline will provide an interactive easier to read visualization of the data extracted from the windows registries, which is otherwise difficult and time consuming to comb through and understand. ")])])}],F={name:"Home"},I=F,M=Object(g["a"])(I,A,W,!1,null,null,null),H=M.exports,B=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"register"}},[e.show?a("b-form",{staticClass:"m-5",on:{submit:e.onRegister}},[a("b-form-group",{attrs:{label:"Username:"}},[a("b-form-input",{attrs:{required:"",placeholder:"Enter username"},model:{value:e.form.name,callback:function(t){e.$set(e.form,"name",t)},expression:"form.name"}})],1),a("b-form-group",{attrs:{label:"Password:"}},[a("b-form-input",{attrs:{type:"password",required:"",placeholder:"Enter password"},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),a("password",{attrs:{"strength-meter-only":!0},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),a("b-form-input",{attrs:{type:"password",required:"",placeholder:"Re-enter password"},model:{value:e.form.passwordconfirm,callback:function(t){e.$set(e.form,"passwordconfirm",t)},expression:"form.passwordconfirm"}}),a("div",{attrs:{id:"messages"}})],1),a("b-button",{attrs:{type:"register",variant:"primary"}},[e._v("Register")])],1):e._e()],1)},q=[],D=(a("b0c0"),a("f20a")),N=a.n(D),U={name:"RegisterForm",components:{Password:N.a},data:function(){return{form:{name:"",password:"",passwordconfirm:""},show:!0}},methods:{onRegister:function(e){e.preventDefault();var t="https://seeshells.herokuapp.com/";if(this.form.password==this.form.passwordconfirm){var a='{"username":"'+this.form.name+'", "password":"'+this.form.password+'"}',s=t+"register",r=new XMLHttpRequest;r.open("POST",s,!1),r.setRequestHeader("Content-type","application/json; charset=UTF-8");try{r.send(a);var n=JSON.parse(r.responseText);1==n.success?document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-info alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Registration sent! </strong>You must wait for a current administrator to approve you now. </div>'):document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong>'+n.error+" Please try again later. </div>")}catch(i){alert(i.message)}}else document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Passwords don\'t match! </strong>Re-enter the passwords. </div>')}}},z=U,J=(a("7197"),Object(g["a"])(z,B,q,!1,null,null,null)),X=J.exports,K=function(){var e=this,t=e.$createElement;e._self._c;return e._m(0)},Y=[function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"team"}},[s("h1",[e._v("Meet the Team")]),s("h4",{staticClass:"section-subheading text-muted"},[e._v("The SeeShells application is a Senior Design project built by five Computer Science students at UCF.")]),s("div",{staticClass:"column"},[s("div",{staticClass:"card"},[s("img",{staticStyle:{width:"100%"},attrs:{src:a("cf05"),alt:"Sara"}}),s("div",{staticClass:"container"},[s("h2",[e._v("Sara Frackiewicz")]),s("p",{staticClass:"title"},[e._v("Project Manager, API, and Website")])])])]),s("div",{staticClass:"column"},[s("div",{staticClass:"card"},[s("img",{staticStyle:{width:"100%"},attrs:{src:a("cf05"),alt:"Klayton"}}),s("div",{staticClass:"container"},[s("h2",[e._v(" Klayton Killough ")]),s("p",{staticClass:"title"},[e._v("WPF")])])])]),s("div",{staticClass:"column"},[s("div",{staticClass:"card"},[s("img",{staticStyle:{width:"100%"},attrs:{src:a("cf05"),alt:"Aleks"}}),s("div",{staticClass:"container"},[s("h2",[e._v("Aleksander Stoyanov")]),s("p",{staticClass:"title"},[e._v("WPF")])])])]),s("div",{staticClass:"column"},[s("div",{staticClass:"card"},[s("img",{staticStyle:{width:"100%"},attrs:{src:a("cf05"),alt:"Bridget"}}),s("div",{staticClass:"container"},[s("h2",[e._v("Bridget Woodye")]),s("p",{staticClass:"title"},[e._v("WPF")])])])]),s("div",{staticClass:"column"},[s("div",{staticClass:"card"},[s("img",{staticStyle:{width:"100%"},attrs:{src:a("cf05"),alt:"Yara"}}),s("div",{staticClass:"container"},[s("h2",[e._v("Yara As-Saidi")]),s("p",{staticClass:"title"},[e._v("WPF and Website")])])])])])}],G={name:"team"},V=G,Q=(a("7bbb"),Object(g["a"])(V,K,Y,!1,null,null,null)),Z=Q.exports,ee=function(){var e=this,t=e.$createElement;e._self._c;return e._m(0)},te=[function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"about"}},[a("h1",[e._v("About Our Project")]),a("p",{attrs:{align:"left"}},[a("br"),e._v(" SeeShells is essentially an information extract software. The objective is to create a standalone open source executable that can run both online and offline. It will extract and parse through Windows Registry information. This data will then be converted into two forms. The first is a csv file that will contain all the raw data we obtain. The second is a human readable timeline. The timeline will provide an interactive easier to read visualization of the data extracted from the windows registries, which is otherwise difficult and time consuming to comb through and understand. The parsing and extraction of information has a slightly different process for each of the windows versions including Windows XP, Windows Vista Windows 7,8,8.1 and 10. In order to create a robust application we have set up a server to store database information on parsing different registry versions. ")]),a("p",{attrs:{align:"left"}},[e._v(" In the long run our hope for this software is that it will expedite the process of extracting, parsing, and presenting the registry information in a way that is condensed and easily understandable. We hope others will benefit from our interactive timeline generated from the ShellBag information and we hope to make a great impact on the digital forensics community. "),a("br"),a("br")]),a("p"),a("h2",{attrs:{align:"left"}},[e._v("Objectives of SeeShells")]),a("p"),a("ul",[a("li",{attrs:{align:"left"}},[e._v(" Create a standalone WPF application that can extract and parse ShellBags registry information. ")]),a("li",{attrs:{align:"left"}},[e._v(" Support multiple Windows Operating Systems as well as reading both live system registries and offline hives. ")]),a("li",{attrs:{align:"left"}},[e._v(" Draw a human-readable, graphical timeline for users to look at as well as a document that can be inserted into a technical report. ")]),a("li",{attrs:{align:"left"}},[e._v(" Set up and utilize a server to store database information on parsing different registry versions to create a robust application. ")])])])}],ae={name:"about"},se=ae,re=Object(g["a"])(se,ee,te,!1,null,null,null),ne=re.exports,ie=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{attrs:{id:"login"}},[e.show?a("b-form",{staticClass:"m-5",on:{submit:e.onLogin}},[a("b-form-group",{attrs:{label:"Username:"}},[a("b-form-input",{attrs:{required:"",placeholder:"Enter username"},model:{value:e.form.name,callback:function(t){e.$set(e.form,"name",t)},expression:"form.name"}})],1),a("b-form-group",{attrs:{label:"Password:"}},[a("b-form-input",{attrs:{type:"password",required:"",placeholder:"Enter password"},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),a("div",{attrs:{id:"messages"}})],1),a("b-button",{attrs:{type:"login",variant:"primary"}},[e._v("Login")])],1):e._e()],1)},oe=[],le={name:"LoginForm",data:function(){return{form:{name:"",password:""},show:!0}},methods:{onLogin:function(e){e.preventDefault();var t="https://seeshells.herokuapp.com/",a=t+"login",s='{"username":"'+this.form.name+'", "password":"'+this.form.password+'"}',r=new XMLHttpRequest;r.open("POST",a,!1),r.setRequestHeader("Content-type","application/json; charset=UTF-8");try{r.send(s);var n=JSON.parse(r.responseText);1==n.success?(this.$session.start(),this.$session.set("id",n.session),this.$session.set("user",n.user.id),this.$router.push("/SeeShells/"),location.reload()):document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to login. Please try again. </div>')}catch(i){document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to login. Please try again. </div>')}}}},ce=le,de=(a("013b"),Object(g["a"])(ce,ie,oe,!1,null,null,null)),ue=de.exports,me="/SeeShells/",pe=[{path:me,component:H},{path:me+"register",component:X},{path:me+"team",component:Z},{path:me+"about",component:ne},{path:me+"login",component:ue}],fe=pe;s["default"].config.productionTip=!1,s["default"].use(r["a"]),s["default"].use(n["a"]),s["default"].use(i["a"]),s["default"].use(l.a);var he=new i["a"]({mode:"history",routes:fe});new s["default"]({router:he,render:function(e){return e(R)}}).$mount("#app")},7197:function(e,t,a){"use strict";var s=a("d201"),r=a.n(s);r.a},"7bbb":function(e,t,a){"use strict";var s=a("5251"),r=a.n(s);r.a},"85ec":function(e,t,a){},cf05:function(e,t,a){e.exports=a.p+"img/logo.9b01295e.png"},d201:function(e,t,a){},e9fb:function(e,t,a){}});
//# sourceMappingURL=app.b3176eb6.js.map