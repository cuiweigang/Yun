#-*-encoding:utf-8 -*-
# 二维码生成示例
import qrcode

qr = qrcode.QRCode(
    version=1,
    error_correction=qrcode.constants.ERROR_CORRECT_L,
    box_size=5, #大小
    border=1, #边框粗细
)

#简单方法
img=qrcode.make("http://kd.yintai.com/shop/share?shopid=1000000")
img.save("ercode1.jpg")

#高级用法
qr.add_data('http://kd.yintai.com/shop/share?shopid=1000000')
qr.make(fit=True)
img=qr.make_image()

#保存二维码
img.save("ercode2.jpg")
