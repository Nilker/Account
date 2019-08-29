	
        var Sys = {};

        var ua = navigator.userAgent.toLowerCase();

        var s;

        (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :

        (s = ua.match(/trident\/([\d.]+)/)) ? Sys.ie = s[1] :

        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :

        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :

        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :

        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;


        if (Sys.ie){
			var s = ""
			s += "<object id=HAccountostil1 height=768 width='100%' style='LEFT: 0px; TOP: 0px'  classid='clsid:FF1FE7A0-0578-4FEE-A34E-FB21B277D561' codebase='./common/HAccountostil.cab#version=4,1,1,0'>"
			s +="<param name='_ExtentX' value='6350'><param name='_ExtentY' value='6350'>"
			s +="</OBJECT>"
			document.write(s);
		} 

        if (Sys.firefox || Sys.chrome){
			var s = ""
			s += "<object id=HAccountostil1 TYPE='application/x-itst-activex'  clsid='{FF1FE7A0-0578-4FEE-A34E-FB21B277D561}' " +
					"event_NotifyCtrlReady='NotifyCtrlReady1' event_NotifyDocOpened='NotifyDocOpened1' progid='' height=768 width='100%' style='LEFT: 0px; TOP: 0px' progid='./common/HAccountostil.cab#version=4,1,1,0'>"
			s +="<param name='_ExtentX' value='6350'><param name='_ExtentY' value='6350'>"
			s +="</OBJECT>"
			document.write(s);
		} 

		