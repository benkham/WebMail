import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { WebMailService } from './webmail.module';
import { EmailInput } from './webmail.module';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  senders: string[];
  emailInput: EmailInput;
  emailForm: FormGroup;
  response: any;
  error: any;

  constructor(private webMailService: WebMailService) {
    this.createForm();
  }

  createForm() {
    this.emailForm = new FormGroup({
      'toAddresses': new FormControl('', [Validators.required,
      Validators.minLength(10), Validators.maxLength(200)]),
      'ccAddresses': new FormControl('', [Validators.maxLength(200)]),
      'bccAddresses': new FormControl('', [Validators.maxLength(200)]),
      'subject': new FormControl('',
        [Validators.required, Validators.minLength(10), Validators.maxLength(100)]),
      'body': new FormControl('',
        [Validators.required, Validators.minLength(10), Validators.maxLength(500)])
    });
  }

  get toAddresses() { return this.emailForm.get('toAddresses'); }
  get ccAddresses() { return this.emailForm.get('ccAddresses'); }
  get bccAddresses() { return this.emailForm.get('bccAddresses'); }
  get subject() { return this.emailForm.get('subject'); }
  get body() { return this.emailForm.get('body'); }

  getSenders(): void {
    this.webMailService.apiWebMailGet()
      .subscribe(senders => this.senders = senders);
  }

  async ngOnInit() {
    this.getSenders();
  }

  sendEmail(email: EmailInput): void {
    this.webMailService.apiWebMailPost(email).subscribe(
      response => this.response = 'Email has been sent successfully!',
      error => this.error = 'An error occurred! ' + error.response);
  }

  onClickEmail() {
    this.emailInput = {
      toAddresses: this.emailForm.get('toAddresses').value,
      ccAddresses: this.emailForm.get('ccAddresses').value,
      bccAddresses: this.emailForm.get('bccAddresses').value,
      subject: this.emailForm.get('subject').value,
      body: this.emailForm.get('body').value
    };

    this.sendEmail(this.emailInput);
  }

  onClickReset() {
    this.error = undefined;
    this.response = undefined;
  }
}
