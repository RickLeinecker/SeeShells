(function(e){function t(t){for(var s,o,i=t[0],l=t[1],d=t[2],c=0,p=[];c<i.length;c++)o=i[c],Object.prototype.hasOwnProperty.call(a,o)&&a[o]&&p.push(a[o][0]),a[o]=0;for(s in l)Object.prototype.hasOwnProperty.call(l,s)&&(e[s]=l[s]);u&&u(t);while(p.length)p.shift()();return n.push.apply(n,d||[]),r()}function r(){for(var e,t=0;t<n.length;t++){for(var r=n[t],s=!0,i=1;i<r.length;i++){var l=r[i];0!==a[l]&&(s=!1)}s&&(n.splice(t--,1),e=o(o.s=r[0]))}return e}var s={},a={app:0},n=[];function o(t){if(s[t])return s[t].exports;var r=s[t]={i:t,l:!1,exports:{}};return e[t].call(r.exports,r,r.exports,o),r.l=!0,r.exports}o.m=e,o.c=s,o.d=function(e,t,r){o.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:r})},o.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},o.t=function(e,t){if(1&t&&(e=o(e)),8&t)return e;if(4&t&&"object"===typeof e&&e&&e.__esModule)return e;var r=Object.create(null);if(o.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var s in e)o.d(r,s,function(t){return e[t]}.bind(null,s));return r},o.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return o.d(t,"a",t),t},o.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},o.p="/SeeShells/";var i=window["webpackJsonp"]=window["webpackJsonp"]||[],l=i.push.bind(i);i.push=t,i=i.slice();for(var d=0;d<i.length;d++)t(i[d]);var u=l;n.push([0,"chunk-vendors"]),r()})({0:function(e,t,r){e.exports=r("56d7")},"013b":function(e,t,r){"use strict";var s=r("e9fb"),a=r.n(s);a.a},"034f":function(e,t,r){"use strict";var s=r("85ec"),a=r.n(s);a.a},3048:function(e,t,r){"use strict";var s=r("3815"),a=r.n(s);a.a},3815:function(e,t,r){},"56d7":function(e,t,r){"use strict";r.r(t);r("e260"),r("e6cf"),r("cca6"),r("a79d");var s=r("2b0e"),a=r("5f5b"),n=r("b1e0"),o=(r("f9e3"),r("2dd8"),r("8c4f")),i=r("0628"),l=r.n(i),d=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"app"}},[r("NavigationBar"),r("div",{attrs:{id:"page"}},[r("router-view")],1)],1)},u=[],c=function(){var e=this,t=e.$createElement,s=e._self._c||t;return s("div",{attrs:{id:"nav"}},[s("b-navbar",{attrs:{toggleable:"lg",type:"dark",variant:"dark"}},[s("img",{attrs:{alt:"Vue logo",src:r("cf05"),width:"75",height:"75"}}),s("router-link",{attrs:{to:"/SeeShells/"}},[s("b-navbar-brand",[e._v("SeeShells")])],1),s("b-navbar-toggle",{attrs:{target:"nav-collapse"}}),s("b-collapse",{attrs:{id:"nav-collapse","is-nav":""}},[s("b-navbar-nav",[s("b-nav-item",{attrs:{href:"#"}},[e._v("About")]),s("b-nav-item",{attrs:{href:"#"}},[e._v("Download the Program")]),s("b-nav-item",{attrs:{href:"#"}},[e._v("Developer Team")]),s("b-nav-item",{attrs:{href:"https://github.com/RickLeinecker/SeeShells"}},[e._v("GitHub Page")])],1),s("b-navbar-nav",{staticClass:"ml-auto"},[s("b-nav-item-dropdown",{attrs:{right:""},scopedSlots:e._u([{key:"button-content",fn:function(){return[s("em",[e._v("Administrative")])]},proxy:!0}])},[e.sessionExists?s("div",[s("LoggedIn")],1):s("div",[s("LoggedOut")],1)])],1)],1)],1)],1)},p=[],m=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"items"}},[r("b-dropdown-item",[e._v("Approve New Admins")]),r("b-dropdown-item",[e._v("Add New Script")]),r("b-dropdown-item",{on:{click:e.Logout}},[e._v("Logout")])],1)},f=[],g={name:"LoggedIn",methods:{Logout:function(){this.$session.destroy();var e="https://seeshells.herokuapp.com/",t=e+"logout",r=new XMLHttpRequest;r.open("GET",t,!1),r.setRequestHeader("Content-type","application/json; charset=UTF-8");try{r.send()}catch(s){alert(s.message)}location.reload()}}},v=g,b=r("2877"),h=Object(b["a"])(v,m,f,!1,null,null,null),w=h.exports,y=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"items"}},[r("b-dropdown-item",[r("router-link",{attrs:{to:"/SeeShells/register"}},[e._v("Register")])],1),r("b-dropdown-item",[r("router-link",{attrs:{to:"/SeeShells/login"}},[e._v("Login")])],1)],1)},_=[],S={name:"LoggedOut",methods:{}},x=S,O=Object(b["a"])(x,y,_,!1,null,null,null),j=O.exports,E={name:"NavigationBar",computed:{sessionExists:function(){return this.$session.exists()}},components:{LoggedIn:w,LoggedOut:j}},L=E,k=(r("3048"),Object(b["a"])(L,c,p,!1,null,"678d4fd4",null)),$=k.exports,P={name:"App",components:{NavigationBar:$}},T=P,R=(r("034f"),Object(b["a"])(T,d,u,!1,null,null,null)),H=R.exports,M=function(){var e=this,t=e.$createElement;e._self._c;return e._m(0)},q=[function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"home"}},[r("p",[e._v("home!!!")])])}],A={name:"Home"},B=A,I=Object(b["a"])(B,M,q,!1,null,null,null),F=I.exports,N=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"register"}},[e.show?r("b-form",{staticClass:"m-5",on:{submit:e.onRegister}},[r("b-form-group",{attrs:{label:"Username:"}},[r("b-form-input",{attrs:{required:"",placeholder:"Enter username"},model:{value:e.form.name,callback:function(t){e.$set(e.form,"name",t)},expression:"form.name"}})],1),r("b-form-group",{attrs:{label:"Password:"}},[r("b-form-input",{attrs:{type:"password",required:"",placeholder:"Enter password"},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),r("password",{attrs:{"strength-meter-only":!0},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),r("b-form-input",{attrs:{type:"password",required:"",placeholder:"Re-enter password"},model:{value:e.form.passwordconfirm,callback:function(t){e.$set(e.form,"passwordconfirm",t)},expression:"form.passwordconfirm"}}),r("div",{attrs:{id:"messages"}})],1),r("b-button",{attrs:{type:"register",variant:"primary"}},[e._v("Register")])],1):e._e()],1)},C=[],U=(r("b0c0"),r("f20a")),D=r.n(U),J={name:"RegisterForm",components:{Password:D.a},data:function(){return{form:{name:"",password:"",passwordconfirm:""},show:!0}},methods:{onRegister:function(e){e.preventDefault();var t="https://seeshells.herokuapp.com/";if(this.form.password==this.form.passwordconfirm){var r='{"username":"'+this.form.name+'", "password":"'+this.form.password+'"}',s=t+"register",a=new XMLHttpRequest;a.open("POST",s,!1),a.setRequestHeader("Content-type","application/json; charset=UTF-8");try{a.send(r);var n=JSON.parse(a.responseText);1==n.success?document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-info alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Registration sent! </strong>You must wait for a current administrator to approve you now. </div>'):document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong>'+n.error+" Please try again later. </div>")}catch(o){alert(o.message)}}else document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Passwords don\'t match! </strong>Re-enter the passwords. </div>')}}},X=J,G=(r("7197"),Object(b["a"])(X,N,C,!1,null,null,null)),V=G.exports,Y=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"login"}},[e.show?r("b-form",{staticClass:"m-5",on:{submit:e.onLogin}},[r("b-form-group",{attrs:{label:"Username:"}},[r("b-form-input",{attrs:{required:"",placeholder:"Enter username"},model:{value:e.form.name,callback:function(t){e.$set(e.form,"name",t)},expression:"form.name"}})],1),r("b-form-group",{attrs:{label:"Password:"}},[r("b-form-input",{attrs:{type:"password",required:"",placeholder:"Enter password"},model:{value:e.form.password,callback:function(t){e.$set(e.form,"password",t)},expression:"form.password"}}),r("div",{attrs:{id:"messages"}})],1),r("b-button",{attrs:{type:"login",variant:"primary"}},[e._v("Login")])],1):e._e()],1)},z=[],K={name:"LoginForm",data:function(){return{form:{name:"",password:""},show:!0}},methods:{onLogin:function(e){e.preventDefault();var t="https://seeshells.herokuapp.com/",r=t+"login",s='{"username":"'+this.form.name+'", "password":"'+this.form.password+'"}',a=new XMLHttpRequest;a.open("POST",r,!1),a.setRequestHeader("Content-type","application/json; charset=UTF-8");try{a.send(s);var n=JSON.parse(a.responseText);1==n.success?(this.$session.start(),this.$session.set("id",n.session),this.$session.set("user",n.user.id),this.$router.push("/SeeShells/"),location.reload()):document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to login. Please try again. </div>')}catch(o){document.getElementById("messages").insertAdjacentHTML("afterend",'<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to login. Please try again. </div>')}}}},Q=K,W=(r("013b"),Object(b["a"])(Q,Y,z,!1,null,null,null)),Z=W.exports,ee="/SeeShells/",te=[{path:ee,component:F},{path:ee+"register",component:V},{path:ee+"login",component:Z}],re=te;s["default"].config.productionTip=!1,s["default"].use(a["a"]),s["default"].use(n["a"]),s["default"].use(o["a"]),s["default"].use(l.a);var se=new o["a"]({mode:"history",routes:re});new s["default"]({router:se,render:function(e){return e(H)}}).$mount("#app")},7197:function(e,t,r){"use strict";var s=r("d201"),a=r.n(s);a.a},"85ec":function(e,t,r){},cf05:function(e,t,r){e.exports=r.p+"img/logo.9b01295e.png"},d201:function(e,t,r){},e9fb:function(e,t,r){}});
//# sourceMappingURL=app.689cdafe.js.map