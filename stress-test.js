import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '2m', target: 100 },
        { duration: '2m', target: 200 },
        { duration: '2m', target: 300 },
        { duration: '2m', target: 400 },
        { duration: '2m', target: 600 },
        { duration: '2m', target: 800 },
        { duration: '2m', target: 700 },
        { duration: '2m', target: 1000 },
        { duration: '2m', target: 1200 },
        { duration: '2m', target: 1400 },
        { duration: '2m', target: 1600 },
        { duration: '2m', target: 1800 },
        { duration: '2m', target: 2000 }
    ],
};

export default function () {
    let response = http.get('http://localhost:5000/api/DataRecord/data');
    sleep(1);
}
