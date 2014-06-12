    #-*- coding: UTF-8 -*-
    #  Copyright (c) 2013 The CCP project authors. All Rights Reserved.
    #
    #  Use of this source code is governed by a Beijing Speedtong Information Technology Co.,Ltd license
    #  that can be found in the LICENSE file in the root of the web site.
    #
    #   http://www.cloopen.com
    #
    #  An additional intellectual property rights grant can be found
    #  in the file PATENTS.  All contributing project authors may
    #  be found in the AUTHORS file in the root of the source tree.

import md5
import base64
import datetime
import urllib2

class RestAPI:
    
    ISJSON_MODEL = True
    
    HOSTNAME = "https://sandboxapp.cloopen.com"
    PORT = "8883"
    SOFTVER = "2013-12-26";
    
    nowdate = "111";

    # @brief                  创建子账号
    # @param accountSid       主账号
    # @param authToken        主账号令牌
    # @param appId            应用id
    # @param friendlyName     申请的邮箱
    def CreateSubAccount(self, accountSid, authToken, appId, friendlyName):
        # create url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        #append the timestamp
        sig = accountSid + authToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/Accounts/" + accountSid + "/SubAccounts?sig=" + signature
        src = accountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
        
        req.add_header("Authorization", auth)
        
        #create body string
        body ='''<?xml version="1.0" encoding="utf-8"?><SubAccount><appId>%s</appId>\
            <friendlyName>%s</friendlyName>\
            </SubAccount>\
            '''%(appId, friendlyName)
         
        if self.ISJSON_MODEL == True:   
            # if this model is Json ..then do next code 
            body = '''{"friendlyName": "%s", "appId": "%s"}'''%(friendlyName,appId)
            print (body);
        req.add_data(body)
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data
    
    # @brief                    发送短信
    # @param accountSid         主账号
    # @param authToken          主账号令牌
    # @param subAccountSid      子账号    
    # @param appId              应用id
    # @param to                 接收短信的电话
    # @param body               短信内容
    # @param msgType            信息类型
    def SendSMS(self, accountSid, authToken, subAccountSid, appId, to, body, msgType):
        # url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        sig = accountSid + authToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/Accounts/" + accountSid + "/SMS/Messages?sig=" + signature
        src = accountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
            
        req.add_header("Authorization", auth)

        bodyData ='''<?xml version='1.0' encoding='utf-8'?><SMSMessage>\
                 <to>%s</to>\
                 <body>%s</body>\
                 <msgType>%s</msgType>\
                 <appId>%s</appId>\
                 <subAccountSid>%s</subAccountSid>\
                 </SMSMessage>\
              '''%(to, body, msgType, appId, subAccountSid)
              
        if self.ISJSON_MODEL == True:   
            # if this model is Json ..then do next code 
            bodyData = '''{"to": "%s", "body": "%s", "msgType": "%s", "appId": "%s", "subAccountSid": "%s"}'''%(to, body, msgType, appId, subAccountSid)
            
        print bodyData
        req.add_data(bodyData)
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data
    
    # @brief                    双向回拨
    # @param subAccountSid      子账号    
    # @param subToken           子账号令牌
    # @param voipAccount        VoIP账号
    # @param fromPhone          主叫电话
    # @param to                 被叫电话
    def CallBack(self, subAccountSid, subToken, voipAccount, fromPhone, to):        
        # url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        sig = subAccountSid + subToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/SubAccounts/" + subAccountSid + "/Calls/Callback?sig=" + signature
        src = subAccountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
            
        req.add_header("Authorization", auth)
        
        body ='''<?xml version='1.0' encoding='utf-8'?><CallBack>\
                 <from>%s</from>\
                 <to>%s</to>\
                 </CallBack>\
              '''%(fromPhone, to)
              
        if self.ISJSON_MODEL == True:
            # if this model is Json ..then do next code 
            body = '''{"from": "%s", "to": "%s"}'''%(fromPhone, to)
            
        print body
        req.add_data(body)
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data
    
    # @brief                    账户信息查询
    # @param accountSid         主账号
    # @param authToken          主账号令牌
    def QueryAccountInfo(self, accountSid, authToken):
        #url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        #append the timestamp
        sig = accountSid + authToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/Accounts/" + accountSid + "/AccountInfo?sig=" + signature
        src = accountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
            
        req.add_header("Authorization", auth)
        
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data

    # @brief                营销外呼
    # @param accountSid     主账号
    # @param authToken      主账号令牌
    # @param appId          应用id
    # @param mediaName      多媒体文件名字
    # @param playTimes      播放次数
    # @param to             要外呼的号码
    def LandingCalls(self, accountSid, authToken, appId, mediaName, playTimes, to):
        # create url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        #append the timestamp
        sig = accountSid + authToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/Accounts/" + accountSid + "/Calls/LandingCalls?sig=" + signature
        src = accountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
            
        req.add_header("Authorization", auth)
        
        #create body string
        body ='''<?xml version="1.0" encoding="utf-8"?><LandingCall><appId>%s</appId><mediaName>%s</mediaName>\
            '''%(appId, mediaName)

        body += '''<playTimes>%s</playTimes><to>%s</to></LandingCall>\
            '''%(playTimes, to)
            
        if self.ISJSON_MODEL == True:
            # if this model is Json ..then do next code 
            body = '''{"appId": "%s", "mediaName": "%s", "playTimes": "%s", "to": "%s"}'''%(appId, mediaName, playTimes, to)
        
        print body
        req.add_data(body)
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data

    # @brief                    语音验证码
    # @param accountSid         主账号
    # @param authToken          主账号令牌
    # @param appId              应用id
    # @param verifyCode         验证码内容，为数字和英文字母，不区分大小写，长度4-20位
    # @param playTimes          播放次数，1－3次
    # @param to                 接收号码
    def VoiceVerifyCode(self, accountSid, authToken, appId, verifyCode, playTimes, to):
        #url content
        nowdate = datetime.datetime.now()
        timestamp = nowdate.strftime("%Y%m%d%H%M%S")
        #append the timestamp
        sig = accountSid + authToken + timestamp;
        signature = md5.new(sig).hexdigest().upper()
        url = self.HOSTNAME + ":" + self.PORT + "/" + self.SOFTVER + "/Accounts/" + accountSid + "/Calls/VoiceVerify?sig=" + signature
        src = accountSid + ":" + timestamp;
        auth = base64.encodestring(src).strip()
        req = urllib2.Request(url)
        
        self.setHttpHeader(req)
        
        req.add_header("Authorization", auth)

        #create body string
        body ='''<?xml version="1.0" encoding="utf-8"?><VoiceVerify><appId>%s</appId>\
            <verifyCode>%s</verifyCode>\
            <playTimes>%s</playTimes>\
            <to>%s</to></VoiceVerify>\
            '''%(appId, verifyCode, playTimes, to)
            
        if self.ISJSON_MODEL == True:
            # if this model is Json ..then do next code 
            body = '''{"appId": "%s", "verifyCode": "%s", "playTimes": "%s", "to": "%s"}'''%(appId, verifyCode, playTimes, to)
            
        req.add_data(body)
        try:
            res = urllib2.urlopen(req);
            data = res.read()
            res.close()
        except urllib2.HTTPError, error:
            data = error.read()
            error.close()
        return data


    def setHttpHeader(self,req):
        if self.ISJSON_MODEL == True:
            req.add_header("Accept", "application/json")
            req.add_header("Content-Type", "application/json;charset=utf-8")
            
        else:
            req.add_header("Accept", "application/xml")
            req.add_header("Content-Type", "application/xml;charset=utf-8")
    

