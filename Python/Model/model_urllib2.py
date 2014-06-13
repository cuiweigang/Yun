#-*- coding:utf-8 -*-
# Author:cuiweigang
# Description:urllib2模块学习

import urllib
import urllib2
import datetime
import md5
import base64

HOSTNAME = "https://sandboxapp.cloopen.com"
PORT = "8883"
SOFTVER = "2013-12-26";
nowdate = "111";

def AccountInfo(accountSid,authToken):
	
	# 获取系统当前时间
	date=datetime.datetime.now()
	
	# 系统时间格式化字符串
	timestamp=date.strftime("%Y%m%d%H%M%S")
	sig=accountSid+authToken+timestamp
	
	# MD5哈希
	signature=md5.new(sig).hexdigest().upper()
	url="%s:%s/%s/Accounts/%s/AccountInfo?sig=%s" %(HOSTNAME,PORT,SOFTVER,accountSid,signature)
	src=accountSid+":"+timestamp;

	#64位字符编码
	auth=base64.encodestring(src).strip()

	#获取Request请求
	req=urllib2.Request(url)

	#添加头部信息
	req.add_header("accept","application/json")
	req.add_header("Content-Type", "application/json;charset=utf-8")
	req.add_header("Authorization",auth)
	#请求网络
	res=urllib2.urlopen(req)
	#获取结果
	data=res.read()
	#关闭网络请求
	res.close()
	return data

print AccountInfo("accountSid","authToken")

