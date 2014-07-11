# encoding:GBK

import httplib
import re,sys
import os
import HTMLParser  
import urlparse  
import urllib  
import urllib2  
import cookielib  
import string 


# ��ȡƷ��ID
def achieveBrandId(path):
	m=re.search(r"\d+",path)
	if m:
		return m.group(0)
	return 0

#���ͼƬ�Ƿ����
def checkExits(path):
	url="p10.ytrss.com"
	conn=httplib.HTTPConnection(url)
	conn.request("GET",path)
	r1=conn.getresponse()
	return (r1.status,achieveBrandId(path))

def request(url):
    ret = urlparse.urlparse(url)
    if ret.scheme == 'http':
        conn = httplib.HTTPConnection(ret.netloc)
    elif ret.scheme == 'https':
        conn = httplib.HTTPSConnection(ret.netloc)
        
    url = ret.path
    if ret.query: url += '?' + ret.query
    if ret.fragment: url += '#' + ret.fragment
    if not url: url = '/'
    
    cookie = r"addressId=a298e166-788b-47b1-bc1b-1b46ab3a7e6a; ASP.NET_SessionId=skbzkx5apsppmjszpk3top55; sid=id=23efe7b4-9143-4930-854e-b740224b44af; s_cc=true; s_fid=70632E6E6BD5221F-3E13DF0A9886A355;"
    conn.request(method='GET', url=url , headers={'Cookie': cookie})
    return conn.getresponse()

# print "��ȡ��ҳ��Ϣ"
#��ҳ
url = 'http://k-d.cc/List/list-50000000-10000000.html'
html_doc = request(url).read()
#��ȡ���еķ�����Ϣ
print "��ȡ���з���..."
categorys=re.findall(r"data-brandid=\"\d{8}\"",html_doc)
categorys=re.findall(r"\d{8}","".join(categorys))
print "������Ϣ��ȡ���"

print "��ȡƷ��ͼƬ��ַ..."

imgs=[]
for category in categorys:
	print "��ȡ%s����ͼƬ..." %category
	urls="http://k-d.cc/List/Filter?N=10000000-50000000&brandId=%s" %category
	print urls
	html_doc=request(urls).read()
	urls=re.findall("/kd/brand/\d+.jpg",html_doc)
	imgs=imgs+urls

# print "��ȡƷ��ͼƬ��ַ���"

print "��ʼ���ͼƬ..."
notexitsImgs=[]
isCheckImgs={}

for img in imgs:
	print "���"+img+"..."
	result= checkExits(img)
	if isCheckImgs.has_key(img):
		print "%s�Ѿ������,����ִ����һ��" %img
		continue
	else:
		isCheckImgs[img]=img

	print result[0],result[1]
	if result[0]==404:
		print "Ʒ��IdΪ%s��ͼƬ������" %result[1]
		imgUrl="<img src='http://p10.ytrss.com/Brand/%s/logo.jpg'/>" %result[1]
		notexitsImgs.append(imgUrl)

# print "����ͼƬ���ļ�"
htmlTemplate='''<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
    <body>
        %s
    </body>
</html>'''

if notexitsImgs:
	html=htmlTemplate %"<br/>".join(notexitsImgs)
	file= open("NotExistBrand.html","w")
	file.write(html)
	print "�в����ڵ�Ʒ��ͼƬ,�����%s�ļ����в鿴" %os.path.join(sys.path[0],file.name)
	file.close()
else:
	print "������,û��ȱ�ٵ�Ʒ��ͼƬ!"	

raw_input("�������������")
