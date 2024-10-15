/*!
 * Compressor.js v1.0.5
 * https://fengyuanchen.github.io/compressorjs
 *
 * Copyright 2018-present Chen Fengyuan
 * Released under the MIT license
 *
 * Date: 2019-01-23T10:53:08.724Z
 */
!function(e,t){"object"==typeof exports&&"undefined"!=typeof module?module.exports=t():"function"==typeof define&&define.amd?define(t):(e=e||self).Compressor=t()}(this,function(){"use strict";function i(e,t){for(var r=0;r<t.length;r++){var a=t[r];a.enumerable=a.enumerable||!1,a.configurable=!0,"value"in a&&(a.writable=!0),Object.defineProperty(e,a.key,a)}}function s(){return(s=Object.assign||function(e){for(var t=1;t<arguments.length;t++){var r=arguments[t];for(var a in r)Object.prototype.hasOwnProperty.call(r,a)&&(e[a]=r[a])}return e}).apply(this,arguments)}function n(i){for(var e=1;e<arguments.length;e++){var n=null!=arguments[e]?arguments[e]:{},t=Object.keys(n);"function"==typeof Object.getOwnPropertySymbols&&(t=t.concat(Object.getOwnPropertySymbols(n).filter(function(e){return Object.getOwnPropertyDescriptor(n,e).enumerable}))),t.forEach(function(e){var t,r,a;t=i,a=n[r=e],r in t?Object.defineProperty(t,r,{value:a,enumerable:!0,configurable:!0,writable:!0}):t[r]=a})}return i}var e,C=(function(e){var t,i,u,f,h,d,n;"undefined"!=typeof window&&(t=window,i=t.HTMLCanvasElement&&t.HTMLCanvasElement.prototype,u=t.Blob&&function(){try{return Boolean(new Blob)}catch(e){return!1}}(),f=u&&t.Uint8Array&&function(){try{return 100===new Blob([new Uint8Array(100)]).size}catch(e){return!1}}(),h=t.BlobBuilder||t.WebKitBlobBuilder||t.MozBlobBuilder||t.MSBlobBuilder,d=/^data:((.*?)(;charset=.*?)?)(;base64)?,/,n=(u||h)&&t.atob&&t.ArrayBuffer&&t.Uint8Array&&function(e){var t,r,a,i,n,o,l,s,c;if(!(t=e.match(d)))throw new Error("invalid data URI");for(r=t[2]?t[1]:"text/plain"+(t[3]||";charset=US-ASCII"),a=!!t[4],i=e.slice(t[0].length),n=a?atob(i):decodeURIComponent(i),o=new ArrayBuffer(n.length),l=new Uint8Array(o),s=0;s<n.length;s+=1)l[s]=n.charCodeAt(s);return u?new Blob([f?l:o],{type:r}):((c=new h).append(o),c.getBlob(r))},t.HTMLCanvasElement&&!i.toBlob&&(i.mozGetAsFile?i.toBlob=function(e,t,r){var a=this;setTimeout(function(){r&&i.toDataURL&&n?e(n(a.toDataURL(t,r))):e(a.mozGetAsFile("blob",t))})}:i.toDataURL&&n&&(i.toBlob=function(e,t,r){var a=this;setTimeout(function(){e(n(a.toDataURL(t,r)))})})),e.exports?e.exports=n:t.dataURLtoBlob=n)}(e={exports:{}},e.exports),e.exports),o={strict:!0,checkOrientation:!0,maxWidth:1/0,maxHeight:1/0,minWidth:0,minHeight:0,width:void 0,height:void 0,quality:.8,mimeType:"auto",convertSize:5e6,beforeDraw:null,drew:null,success:null,error:null},t="undefined"!=typeof window?window:{},c=Array.prototype.slice;var r=/^image\/.+$/;function D(e){return r.test(e)}var m=String.fromCharCode;var u=t.btoa;function f(e){var t,r=new DataView(e);try{var a,i,n;if(255===r.getUint8(0)&&216===r.getUint8(1))for(var o=r.byteLength,l=2;l+1<o;){if(255===r.getUint8(l)&&225===r.getUint8(l+1)){i=l;break}l+=1}if(i){var s=i+10;if("Exif"===function(e,t,r){var a,i="";for(r+=t,a=t;a<r;a+=1)i+=m(e.getUint8(a));return i}(r,i+4,4)){var c=r.getUint16(s);if(((a=18761===c)||19789===c)&&42===r.getUint16(s+2,a)){var u=r.getUint32(s+4,a);8<=u&&(n=s+u)}}}if(n){var f,h,d=r.getUint16(n,a);for(h=0;h<d;h+=1)if(f=n+12*h+2,274===r.getUint16(f,a)){f+=8,t=r.getUint16(f,a),r.setUint16(f,1,a);break}}}catch(e){t=1}return t}var a=/\.\d*(?:0|9){12}\d*$/;function H(e){var t=1<arguments.length&&void 0!==arguments[1]?arguments[1]:1e11;return a.test(e)?Math.round(e*t)/t:e}var h=t.ArrayBuffer,d=t.FileReader,b=t.URL||t.webkitURL,p=/\.\w+$/,l=t.Compressor;return function(){function r(e,t){!function(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}(this,r),this.file=e,this.image=new Image,this.options=n({},o,t),this.aborted=!1,this.result=null,this.init()}var e,t,a;return e=r,a=[{key:"noConflict",value:function(){return window.Compressor=l,r}},{key:"setDefaults",value:function(e){s(o,e)}}],(t=[{key:"init",value:function(){var i=this,n=this.file,e=this.options;if(t=n,"undefined"!=typeof Blob&&(t instanceof Blob||"[object Blob]"===Object.prototype.toString.call(t))){var t,o=n.type;if(D(o))if(b&&d)if(h||(e.checkOrientation=!1),b&&!e.checkOrientation)this.load({url:b.createObjectURL(n)});else{var r=new d,l=e.checkOrientation&&"image/jpeg"===o;(this.reader=r).onload=function(e){var t=e.target.result,r={};if(l){var a=f(t);1<a||!b?(r.url=function(e,t){for(var r,a=[],i=new Uint8Array(e);0<i.length;)a.push(m.apply(null,(r=i.subarray(0,8192),Array.from?Array.from(r):c.call(r)))),i=i.subarray(8192);return"data:".concat(t,";base64,").concat(u(a.join("")))}(t,o),1<a&&s(r,function(e){var t=0,r=1,a=1;switch(e){case 2:r=-1;break;case 3:t=-180;break;case 4:a=-1;break;case 5:t=90,a=-1;break;case 6:t=90;break;case 7:t=90,r=-1;break;case 8:t=-90}return{rotate:t,scaleX:r,scaleY:a}}(a))):r.url=b.createObjectURL(n)}else r.url=t;i.load(r)},r.onabort=function(){i.fail(new Error("Aborted to read the image with FileReader."))},r.onerror=function(){i.fail(new Error("Failed to read the image with FileReader."))},r.onloadend=function(){i.reader=null},l?r.readAsArrayBuffer(n):r.readAsDataURL(n)}else this.fail(new Error("The current browser does not support image compression."));else this.fail(new Error("The first argument must be an image File or Blob object."))}else this.fail(new Error("The first argument must be a File or Blob object."))}},{key:"load",value:function(e){var t=this,r=this.file,a=this.image;a.onload=function(){t.draw(n({},e,{naturalWidth:a.naturalWidth,naturalHeight:a.naturalHeight}))},a.onabort=function(){t.fail(new Error("Aborted to load the image."))},a.onerror=function(){t.fail(new Error("Failed to load the image."))},a.alt=r.name,a.src=e.url}},{key:"draw",value:function(e){var t=this,r=e.naturalWidth,a=e.naturalHeight,i=e.rotate,n=void 0===i?0:i,o=e.scaleX,l=void 0===o?1:o,s=e.scaleY,c=void 0===s?1:s,u=this.file,f=this.image,h=this.options,d=document.createElement("canvas"),m=d.getContext("2d"),b=r/a,p=Math.abs(n)%180==90,g=Math.max(h.maxWidth,0)||1/0,v=Math.max(h.maxHeight,0)||1/0,y=Math.max(h.minWidth,0)||0,w=Math.max(h.minHeight,0)||0,U=Math.max(h.width,0)||r,B=Math.max(h.height,0)||a;if(p){var k=[v,g];g=k[0],v=k[1];var x=[w,y];y=x[0],w=x[1];var M=[B,U];U=M[0],B=M[1]}g<1/0&&v<1/0?g<v*b?v=g/b:g=v*b:g<1/0?v=g/b:v<1/0&&(g=v*b),0<y&&0<w?y<w*b?w=y/b:y=w*b:0<y?w=y/b:0<w&&(y=w*b),U<B*b?B=U/b:U=B*b;var j=-(U=Math.floor(H(Math.min(Math.max(U,y),g))))/2,O=-(B=Math.floor(H(Math.min(Math.max(B,w),v))))/2,A=U,T=B;if(p){var R=[B,U];U=R[0],B=R[1]}d.width=U,d.height=B,D(h.mimeType)||(h.mimeType=u.type);var E="transparent";if(u.size>h.convertSize&&"image/png"===h.mimeType&&(E="#fff",h.mimeType="image/jpeg"),m.fillStyle=E,m.fillRect(0,0,U,B),h.beforeDraw&&h.beforeDraw.call(this,m,d),!this.aborted&&(m.save(),m.translate(U/2,B/2),m.rotate(n*Math.PI/180),m.scale(l,c),m.drawImage(f,j,O,A,T),m.restore(),h.drew&&h.drew.call(this,m,d),!this.aborted)){var L=function(e){t.aborted||t.done({naturalWidth:r,naturalHeight:a,result:e})};d.toBlob?d.toBlob(L,h.mimeType,h.quality):L(C(d.toDataURL(h.mimeType,h.quality)))}}},{key:"done",value:function(e){var t,r,a=e.naturalWidth,i=e.naturalHeight,n=e.result,o=this.file,l=this.image,s=this.options;if(b&&!s.checkOrientation&&b.revokeObjectURL(l.src),n)if(s.strict&&n.size>o.size&&s.mimeType===o.type&&!(s.width>a||s.height>i||s.minWidth>a||s.minHeight>i))n=o;else{var c=new Date;n.lastModified=c.getTime(),n.lastModifiedDate=c,n.name=o.name,n.name&&n.type!==o.type&&(n.name=n.name.replace(p,(t=n.type,"jpeg"===(r=D(t)?t.substr(6):"")&&(r="jpg"),".".concat(r))))}else n=o;this.result=n,s.success&&s.success.call(this,n)}},{key:"fail",value:function(e){var t=this.options;if(!t.error)throw e;t.error.call(this,e)}},{key:"abort",value:function(){this.aborted||(this.aborted=!0,this.reader?this.reader.abort():this.image.complete?this.fail(new Error("The compression process has been aborted.")):(this.image.onload=null,this.image.onabort()))}}])&&i(e.prototype,t),a&&i(e,a),r}()});