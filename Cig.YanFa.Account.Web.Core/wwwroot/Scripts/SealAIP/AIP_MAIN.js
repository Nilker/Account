/****************************************************************************************************

��������OpenFile					���ĵ�
��  ����url							�����Ƿ�����http·����http://127.0.0.1/test.pdf
									Ҳ�����Ǳ����ļ�·����c://test.pdf
									Ҳ�������ļ�����http://127.0.0.1/GetFile.aspx

*****************************************************************************************************/
function OpenFile(url) {
	var AipObj = document.getElementById("HAccountostil1");
	var IsOpen = AipObj.LoadFile(url);
	if (IsOpen != 1) {
		alert("���ĵ�ʧ�ܣ�");
	}
}
/****************************************************************************************************

��������AddSeal						�ֶ����»���д
��  ����usertype					�û����ͣ�0 �����û���1 ����key�û���2 ������key�û���3 �����������û�
		doaction					�������ͣ�0 ���£�1 ��д��
		other						Ԥ�������
											��usertypeΪ1,2ʱ��ֵΪ�û���ʵ����������Ϊ�ջ���ȡ֤���û�����
											��usertypeΪ3ʱ��ֵΪ�������ݡ�
		httpaddr					��������ַ��
											ֻ�е�usertypeΪ2,3ʱ��Щ������Ϊ�ա�

*****************************************************************************************************/
function AddSeal(usertype,doaction,other,httpaddr) {
	var AipObj = document.getElementById("HAccountostil1");
	if(usertype==0){
		var islogin=AipObj.Login("HWSEALDEMO**",4,65535,"DEMO","");
	}else if(usertype==1){
		var islogin=AipObj.Login(other,1,65535,"","");
	}else if(usertype==2){
		var islogin=AipObj.Login(other,3,65535,"","http://"+httpaddr+":8089/inc/seal_interface/");
	}else if(usertype==3){
		var stri="Use-RemotePfx-Login:"+other;
		var strj=stri.length+1;
		var islogin=AipObj.LoginEx("http://"+httpaddr+":8089/inc/seal_interface/", stri, strj);
	}else{
		alert("�û����Ͳ��������Բ����û���ݵ�¼");
		var islogin=AipObj.Login("HWSEALDEMO**",4,65535,"DEMO","");
	}
	if (islogin != 0) {
		alert("����ʧ�ܣ������ţ�" + islogin);
	} else {
		if(doaction==0){
			AipObj.CurrAction = 2568;
		}else if(doaction==1){
			AipObj.CurrAction = 264;
		}
	}
}
/****************************************************************************************************

��������AutoSeal					�Զ�����
��  ����usertype					�û����ͣ�0 �����û���1 ����key�û���2 ������key�û���3 �����������û�
		doaction					�������ͣ�0 ��ͨӡ�£�1 ������£�2�Կ����
		searchtype					��λ����λ�����ͣ�ֻ����ͨӡ��doaction=0ʱ��Ч��0 �������꣬1 ���ֶ�λ
		searchstring				��λ��Ϣ��ֻ����ͨӡ��doaction=0ʱ��Ч
											searchtypeΪ0ʱ��searchstringΪx:y:page��ʽ����200:500:0   xΪ��������1-50000��yΪ��������1-50000��pageΪ����ҳ���0��ʼ
											searchtypeΪ1ʱ��searchstringΪҪ���ҵ������ַ���
		other						Ԥ�������
											��usertypeΪ1,2ʱ��ֵΪ�û���ʵ����������Ϊ�ջ���ȡ֤���û�����
											��usertypeΪ3ʱ��ֵΪ�������ݡ�
		httpaddr					��������ַ��
											ֻ�е�usertypeΪ2,3ʱ��Щ������Ϊ�ա�

*****************************************************************************************************/
function AutoSeal(usertype,doaction,searchtype,searchstring,other,httpaddr) {
	var AipObj = document.getElementById("HAccountostil1");
	
	if(usertype==0){
		var islogin=AipObj.Login("HWSEALDEMO**",4,65535,"DEMO","");
	}else if(usertype==1){
		var islogin=AipObj.Login(other,1,65535,"","");
	}else if(usertype==2){
		var islogin=AipObj.Login(other,3,65535,"","http://"+httpaddr+":8089/inc/seal_interface/");
	}else if(usertype==3){
		var stri="Use-RemotePfx-Login:"+other;
		var strj=stri.length+1;
		var islogin=AipObj.LoginEx("http://"+httpaddr+":8089/inc/seal_interface/", stri, strj);
	}else{
		alert("�û����Ͳ��������Բ����û���ݵ�¼");
		var islogin=AipObj.Login("HWSEALDEMO**",4,65535,"DEMO","");
	}
	if (islogin != 0) {
		alert("����ʧ�ܣ������ţ�" + dtrer);
	} else {
		if(doaction==0){
			var num=AipObj.PageCount;
			var str=searchstring.split(":");
			var page="";
			if(searchtype==0){
				AipObj.AddQifengSeal(0,0+","+str[0]+",0,"+str[1]+",50,"+str[2],"","AUTO_ADD_SEAL_FROM_PATH");
			}else if(searchtype==1){
				AipObj.AddQifengSeal(0,"AUTO_ADD:0,"+num+",0,0,1,"+searchstring+")|(0,","","AUTO_ADD_SEAL_FROM_PATH");
			}
		}else if(doaction==1){
			var num=AipObj.PageCount;
			var page="";
			for(i=1;i<num;i++){
				page+=i+",";
			}
			var bl=100/(num-1);
			AipObj.AddQifengSeal(0,0+",25000,1,3,"+bl+","+page,"","AUTO_ADD_SEAL_FROM_PATH");
		}else if(doaction==2){
			var num=AipObj.PageCount;
			for(i=0;i<num-1;i++){
				AipObj.AddQifengSeal(0,i+",25000,2,3,50,1","","AUTO_ADD_SEAL_FROM_PATH");
			}
		}
	}
}
/****************************************************************************************************

��������SaveTo								�����ĵ�
��  ����savetype							�ĵ����淽ʽ��0 ���汾�أ�1 ���浽������
		filepath							�ĵ�����·����
													savetypeΪ0ʱΪ����·��������Ϊ�գ�Ϊ�ջᵯ����ַ������c:/test/1.pdf
													savetypeΪ1ʱΪ������·��������http://127.0.0.1/getfile.php,��ַΪ�ļ����շ�������ַ�������ļ���FileBlod
		filecode							�ĵ�Ωһ��ʾ��
													savetypeΪ0ʱΪ�ĵ����ͣ�ֵ����Ϊdoc��pdf��aip��word��jpg��gif��bmp��
													savetypeΪ1ʱΪ�ĵ�Ψһ��ʾ���������������յĲ���FileCode

*****************************************************************************************************/
function SaveTo(savetype,filepath,filecode) {
	var AipObj = document.getElementById("HAccountostil1");
	
	if(savetype==0){
		var issave = AipObj.SaveTo(filepath, filecode, 0);
		if (issave == 0) {
			alert("����ʧ�ܣ�");
		}
	}else if(savetype==1){
		AipObj.HttpInit(); //��ʼ��HTTP���档
		AipObj.HttpAddPostString("FileCode", filecode); //�����ϴ�����FileCode�ĵ�Ωһ��ʾ��
		AipObj.HttpAddPostCurrFile("FileBlod"); //�����ϴ���ǰ�ļ�,�ļ���ʶΪFileBlod��
		var ispost = AipObj.HttpPost(filepath); //�ϴ����ݡ�
		if (ispost != 0) {
			alert("�ĵ��ϴ�ʧ�ܣ�������룺" + ispost);
		}
	}else{
		alert("SaveTo������������")
	}
}
/****************************************************************************************************

��������ShowFullScreen					ȫ���鿴
��  ����slog							1Ϊȫ����0Ϊ��ͨ

*****************************************************************************************************/
function ShowFullScreen(slog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.ShowFullScreen = slog;
}
/****************************************************************************************************

��������FilePrint						��ӡ�ĵ�
��  ����plog							0���ٴ�ӡ��1�д�ӡ�Ի���

*****************************************************************************************************/
function FilePrint(plog) {
	var AipObj = document.getElementById("HAccountostil1");
	var isprint = AipObj.PrintDoc(1, plog);
	if (isprint == 0) {
		alert("��ӡʧ�ܣ�");
	}
}
/****************************************************************************************************

��������FileMerge						�ϲ��ļ�
��  ����filepath						Ҫ�ϲ��ļ�·�������ֻΪ�������һ���հ�ҳ
		page							�ļ�Ҫ�����ҳ��,���뵽��һҳֵΪ0

*****************************************************************************************************/
function FileMerge(filepath,page) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.Login("HWSEALDEMO**",4,65535,"DEMO","");
	if(filepath==""){
		var isMerge = AipObj.InsertEmptyPage(page,0,0,0);
	}else{
		var isMerge = AipObj.MergeFile(page,filepath);
	}
	if (isMerge == 0) {
		alert("�ϲ��ĵ�ʧ�ܣ�");
	}
}
/****************************************************************************************************

��������SetPenwidth						������д�ʿ�
��  ������

*****************************************************************************************************/
function SetPenwidth() {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.CurrPenWidth=-1;
}
/****************************************************************************************************

��������SetColor						������д����ɫ
��  ������

*****************************************************************************************************/
function SetColor() {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.CurrPenColor=-1;
}
/****************************************************************************************************

��������SetPressurelevel				������дѹ��
��  ������

*****************************************************************************************************/
function SetPressurelevel() {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.Pressurelevel=-1;
}
/****************************************************************************************************

��������SetAction						�������״̬
��  ����SetLog							����״̬��1 �����2 ����ѡ��

*****************************************************************************************************/
function SetAction(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.CurrAction=SetLog;
}
/****************************************************************************************************

��������SetPageMode						������ͼ
��  ����SetLog							���ò���״̬��1 ԭʼ��С��2 ��Ӧ��ȣ�3 ���ڴ�С��4 ˫ҳ��ʾ��5 �ޱ߿�

*****************************************************************************************************/
function SetPageMode(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	if(SetLog==1){
		AipObj.SetPageMode(1,100);
	}else if(SetLog==2){
		AipObj.SetPageMode(2,100);
	}else if(SetLog==3){
		AipObj.SetPageMode(4,100);
	}else if(SetLog==4){
		AipObj.SetPageMode(8,2);
	}else if(SetLog==5){
		AipObj.SetPageMode(16,1);
	}
}
/****************************************************************************************************

��������ShowToolBar						���ù�����
��  ����SetLog							����״̬��0 ���أ�1 ��ʾ

*****************************************************************************************************/
function ShowToolBar(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.ShowToolBar=SetLog;
}
/****************************************************************************************************

��������ShowDefMenu						���ò˵�
��  ����SetLog							����״̬��0 ���أ�1 ��ʾ

*****************************************************************************************************/
function ShowDefMenu(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.ShowDefMenu=SetLog;
}
/****************************************************************************************************

��������ShowScrollBarButton				���ù�����
��  ����SetLog							����״̬��2 ���ع�������1 ���ع������Ĺ�������0 ��ʾ������

*****************************************************************************************************/
function ShowScrollBarButton(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.ShowScrollBarButton=SetLog;
}
/****************************************************************************************************

��������SetFullScreen					����ȫ��
��  ����SetLog							����״̬��1ȫ����0����

*****************************************************************************************************/
function SetFullScreen(SetLog) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.ShowFullScreen =SetLog;
}
/****************************************************************************************************

��������SearchText						��������
��  ����stxt							Ҫ���ҵ�����
		matchcase						�Ƿ����ִ�Сд
		findnext						����λ�á�0:��ͷ���Բ���;1:������һ��

*****************************************************************************************************/
function SearchText(stxt,matchcase,findnext) {
	var AipObj = document.getElementById("HAccountostil1");
	AipObj.SearchText(stxt,matchcase,findnext);
}


function NotifyCtrlReady1(){
	//alert("��ʼ���¼�ִ��������");
}


function NotifyDocOpened1(){
	//alert("���ĵ���ִ��");
}

function getTempFile(){
	var AipObj = document.getElementById("HAccountostil1");
	var tempFile =  AipObj.GetTempFileName("abc");
	alert(tempFile);
}