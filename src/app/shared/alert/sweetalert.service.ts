import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class SweetAlertService {
	constructor() {
	}

	sweetAlert(title: string, message: string, icon: any, mode: boolean): void {
		Swal.fire(title, message, icon);
	}

	sweetAlertConfirm(title: string, message: string, icon: string, mode: boolean, button1?: string, button2?: string): Promise<any> {
		const btn1 = button1 ? button1 : 'CANCEL';
		const btn2 = button2 ? button2 : 'OK';
		const promise = new Promise((resolve, reject) => {
			Swal.fire({
				title: title,
				text: message,
				//	type: 'warning',
				showCancelButton: true,
				confirmButtonText: btn2,
				cancelButtonText: btn1,
				buttonsStyling: true,
			}).then(result => {
				resolve(result);
			}).catch(err => {
				reject(err);
			});
		});
		return promise;
	}

	// sweetAlertError(errorMessage: string, type?: any, title?: any): void {
	// 	Swal.fire({
	// 		html: type ? '<h1 style="color:black ; font-size: 16px;">' + errorMessage + '</h1>' :
	// 			'<h1 style="color:red ; font-size: 16px;">' + errorMessage + '</h1>',
	// 		type: type ? 'success' : 'error',
	// 		title: title ? title : 'error'
	// 	});
	// }
}
